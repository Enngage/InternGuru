﻿using System;
using System.Threading.Tasks;
using Service.Context;
using Cache;
using Service.Events;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using Core.AutoMapper;
using Core.Loc;
using EmailProvider;
using Entity.Base;
using Service.Exceptions;
using Service.Models;
using Service.Services.Logs;

namespace Service.Services
{
    public abstract class BaseService<TEntity> : IDisposable where TEntity : class, IEntity
    {
        #region Variables

        /// <summary>
        /// Service dependencies
        /// </summary>
        protected IServiceDependencies ServiceDependencies { get; }

        /// <summary>
        /// DB set of current entity types
        /// </summary>
        private IDbSet<TEntity> _entityDbSet;

        /// <summary>
        /// Service context
        /// </summary>
        private IServiceContext _serviceContext;

        #endregion

        #region Properties

        /// <summary>
        /// AppContext 
        /// </summary>
        protected IAppContext AppContext
        {
            get { return ServiceDependencies.AppContext; }
            set { ServiceDependencies.AppContext = value; }
        }

        /// <summary>
        /// DB set representing current entity type
        /// </summary>
        protected IDbSet<TEntity> EntityDbSet
        {
            get
            {
                // try to initialize db set
                if (_entityDbSet == null)
                {
                    _entityDbSet = GetEntitySet();
                }

                if (_entityDbSet == null)
                {
                    throw new NotSupportedException($"DBSet of '{nameof(TEntity)}' entity type was not initialized");
                }
                return _entityDbSet;
            }
            set { _entityDbSet = value; }
        }

        /// <summary>
        /// Cache service
        /// </summary>
        protected ICacheService CacheService => ServiceDependencies.CacheService;

        /// <summary>
        /// Email provider used to send e-mails out into the world
        /// </summary>
        protected IEmailProvider EmailProvider => ServiceDependencies.EmailProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes service abstract instance
        /// </summary>
        /// <param name="serviceDependencies">serviceDependencies</param>
        protected BaseService(IServiceDependencies serviceDependencies)
        {
            ServiceDependencies = serviceDependencies;
        }

        #endregion

        #region Init

        /// <summary>
        /// Initialized the base service with current db set of entity type
        /// </summary>
        /// <returns>DbSet representing current db set</returns>
        public abstract IDbSet<TEntity> GetEntitySet();

        #endregion

        #region IDispose members

        public void Dispose()
        {
            AppContext?.Dispose();
        }

        #endregion

        #region Service context

        public IServiceContext ServiceContext
        {
            private get
            {
                if (_serviceContext == null)
                {
                    // get default context
                    _serviceContext = new ServiceContext();
                }
                return _serviceContext;
            }
            set { _serviceContext = value; }
        }

        #endregion

        #region Sync Save methods

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int SaveChanges()
        {
            return SaveChanges(SaveEventType.Unknown);
        }

        /// <summary>
        /// Updates given object in database
        /// </summary>
        /// <param name="obj">Object to be modified</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Number of updated rows</returns>
        protected int UpdateObject(IDbSet<TEntity> dbSet, TEntity obj)
        {
            return UpdateObject(dbSet, obj, dbSet.Find(obj.ID));
        }

        /// <summary>
        /// Updates given object in database
        /// </summary>
        /// <param name="obj">Object to be modified</param>
        /// <param name="existingObj">Existing object in database</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Number of updated rows</returns>
        protected int UpdateObject(IDbSet<TEntity> dbSet, TEntity obj, TEntity existingObj)
        {
            if (existingObj == null)
            {
                throw new ObjectNotFoundException($"Object of '{nameof(IEntity)}' type with ID = '{obj.GetObjectID()}' was not found.");
            }

            // create clone of existing entity
            var existingObjectClone = new object() as TEntity;
            // ---- IMPORTANT ------ // 
            // In order for cloning to work, Automapper needs to have map from source to target defined like this:
            // cfg.CreateMap<Internship, Internship>();
            // END
            existingObjectClone = MapperProvider.Mapper.Map(existingObj, existingObjectClone);

            // before event
            OnUpdateBefore(obj, existingObjectClone);

            // handle entity variations and constraints
            HandleEntityCodeName(obj);
            HandleEntityVariationFields(SaveEventType.Update, obj, existingObjectClone);

            // check security 
            if (!CanUpdate(obj))
            {
                throw new AuthorizationException($"Authorization required for action '{SaveEventType.Update}'");
            }

            // validate object
            ValidateObject(SaveEventType.Update, obj, existingObjectClone);

            // set additional field's data
            ExtendObject(SaveEventType.Update, obj, existingObjectClone);

            // update existing object with new values
            AppContext.Entry(existingObj).CurrentValues.SetValues(obj);

            // save changes
            var result = SaveChanges(SaveEventType.Update);

            // after event
            OnUpdateAfter(obj, existingObjectClone);

            // touch cache keys
            TouchUpdateKeys(obj);

            return result;
        }

        /// <summary>
        /// Insert new object to database
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Result of the insert action</returns>
        protected IInsertActionResult InsertObject(IDbSet<TEntity> dbSet, TEntity obj)
        {
            // before event
            OnInserBefore(obj);

            // handle entity variations and constraints
            HandleEntityCodeName(obj);
            HandleEntityVariationFields(SaveEventType.Insert, obj, null);

            // validate object
            ValidateObject(SaveEventType.Insert, obj);

            // set additional field's data
            ExtendObject(SaveEventType.Insert, obj);

            // add object to db set
            dbSet.Add(obj);

            // save changes
            var rowsUpdated = SaveChanges(SaveEventType.Insert);

            // touch cache keys
            TouchInsertKeys(obj);

            // after event
            OnInsertAfter(obj);

            return new InsertActionResult()
            {
                UpdatedRows = rowsUpdated,
                ObjectID = obj.ID
            };
        }

        /// <summary>
        /// Deletes given object from database
        /// </summary>
        /// <param name="objectId">ID of the object</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Number of updated rows</returns>
        protected int DeleteObject(IDbSet<TEntity> dbSet, int objectId)
        {
            // get object
            var obj = dbSet.Find(objectId);

            if (obj == null)
            {
                throw new ObjectNotFoundException($"Object of '{nameof(IEntity)}' type with ID = '{objectId}' was not found.");
            }

            // check security 
            if (!CanDelete(obj))
            {
                throw new AuthorizationException($"Authorization required for action '{SaveEventType.Delete}'");
            }

            // before delete event
            OnDeleteBefore(obj);

            // delete object
            dbSet.Remove(obj);

            // save changes
            var result = SaveChanges(SaveEventType.Delete);

            // touch cache keys
            TouchDeleteKeys(obj);

            // after delete event
            OnDeleteAfter(obj);

            return result;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="type">Action type</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int SaveChanges(SaveEventType type)
        {
            try
            {
                // save changes
                var result = AppContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                LogServiceException(ex, type);

                // re-throw
                throw;
            }
        }

        #endregion

        #region Async save methods

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(SaveEventType.Unknown);
        }

        /// <summary>
        /// Updates given object in database
        /// </summary>
        /// <param name="obj">Object to be modified</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Number of updated rows</returns>
        protected async Task<int> UpdateObjectAsync(IDbSet<TEntity> dbSet, TEntity obj)
        {
            return await UpdateObjectAsync(dbSet, obj, dbSet.Find(obj.GetObjectID()));
        }

        /// <summary>
        /// Updates given object in database
        /// </summary>
        /// <param name="obj">New object to be modified</param>
        /// <param name="existingObj">Original object in database</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Number of updated rows</returns>
        protected async Task<int> UpdateObjectAsync(IDbSet<TEntity> dbSet, TEntity obj, TEntity existingObj)
        {
            if (existingObj == null)
            {
                throw new ObjectNotFoundException($"Object of '{nameof(IEntity)}' type with ID = '{obj.GetObjectID()}' cannot be edited because it does not exist.");
            }

            // create clone of existing entity
            var existingObjectClone = new object() as TEntity;
            // ---- IMPORTANT ------ // 
            // In order for cloning to work, Automapper needs to have map from source to target defined like this:
            // cfg.CreateMap<Internship, Internship>();
            // END
            existingObjectClone = MapperProvider.Mapper.Map(existingObj, existingObjectClone);

            // before event
            OnUpdateBefore(obj, existingObjectClone);

            // handle entity variations and constraints
            await HandleEntityCodeNameAsync(obj);
            HandleEntityVariationFields(SaveEventType.Update, obj, existingObjectClone);

            // check security 
            if (!CanUpdate(obj))
            {
                throw new AuthorizationException($"Authorization required for action '{SaveEventType.Update}'");
            }

            // validate object
            ValidateObject(SaveEventType.Update, obj, existingObjectClone);

            // set additional field's data
            ExtendObject(SaveEventType.Update, obj, existingObjectClone);

            // update existing object with new values
            AppContext.Entry(existingObj).CurrentValues.SetValues(obj);

            // save changes
            var result = await SaveChangesAsync(SaveEventType.Update);

            // after event
            OnUpdateAfter(obj, existingObjectClone);

            // touch cache keys
            TouchUpdateKeys(obj);

            return result;
        }

        /// <summary>
        /// Insert new object to database
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Result of the insert action</returns>
        protected async Task<IInsertActionResult> InsertObjectAsync(IDbSet<TEntity> dbSet, TEntity obj)
        {
            // before event
            OnInserBefore(obj);

            // handle entity variations and constraints
            await HandleEntityCodeNameAsync(obj);
            HandleEntityVariationFields(SaveEventType.Insert, obj, null);

            // validate object
            ValidateObject(SaveEventType.Insert, obj);

            // set additional field's data
            ExtendObject(SaveEventType.Insert, obj);

            // add object to db set
            dbSet.Add(obj);

            // save changes
            var updatedRows = await SaveChangesAsync(SaveEventType.Insert);

            // touch cache keys
            TouchInsertKeys(obj);

            // after event
            OnInsertAfter(obj);

            return new InsertActionResult()
            {
                ObjectID = obj.ID,
                UpdatedRows = updatedRows
            };
        }

        /// <summary>
        /// Deletes given object from database
        /// </summary>
        /// <param name="objectId">ID of the object</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns>Number of updated rows</returns>
        protected async Task<int> DeleteObjectAsync(IDbSet<TEntity> dbSet, int objectId)
        {
            // get object
            var obj = dbSet.Find(objectId);

            if (obj == null)
            {
                throw new ObjectNotFoundException($"Object of '{nameof(IEntity)}' type with ID = '{objectId}' was not found.");
            }

            // check security 
            if (!CanDelete(obj))
            {
                throw new AuthorizationException($"Authorization required for action '{SaveEventType.Delete}'");
            }

            // before delete event
            OnDeleteBefore(obj);

            // delete object
            dbSet.Remove(obj);

            // save changes
            var result = await SaveChangesAsync(SaveEventType.Delete);

            // touch cache keys
            TouchDeleteKeys(obj);

            // after delete event
            OnDeleteAfter(obj);

            return result;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="type">Action type</param>
        /// <returns>The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> SaveChangesAsync(SaveEventType type)
        {
            try
            {
                // save changes
                var result = await AppContext.SaveChangesAsync();

                return result;
            }
            catch (Exception ex)
            {
                await LogServiceExceptionAsync(ex, type);

                // re-throw
                throw;
            }
        }

        #endregion

        #region Exception logging

        private void LogServiceException(Exception ex, SaveEventType type)
        {
            // get log service
            var logService = KernelProvider.Get<ILogService>();

            var saveException = new SaveException(GetSaveExceptionText(type), ex);

            // log event
            logService.LogException(saveException, ServiceDependencies.RequestContext.CurrentUrl, ServiceDependencies.User.UserName);
        }

        private async Task LogServiceExceptionAsync(Exception ex, SaveEventType type)
        {
            // make sure to use logger app context to avoid infinite recursion exception if exception occured on DB level

            // get log service
            var logService = KernelProvider.Get<ILogService>();

            var saveException = new SaveException(GetSaveExceptionText(type), ex);

            // log event
            await logService.LogExceptionAsync(saveException, ServiceDependencies.RequestContext.CurrentUrl, ServiceDependencies.User.UserName);
        }

        #endregion

        #region Object edit/insert manipulation methods

        /// <summary>
        /// Validates object
        /// Throw exception if object is not valid
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="newObj">New object (object with new data)</param>
        /// <param name="oldObj">Old object (the one currently saved in db). Available only for update event types</param>
        public virtual void ValidateObject(SaveEventType eventType, TEntity newObj, TEntity oldObj = null)
        {
            // no extra validation required by default
        }

        /// <summary>
        /// Use to set custom field based on additional logic
        /// Only properties in "newObj" will be saved
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="newObj">New object (object with new data). Set properties in this object.</param>
        /// <param name="oldObj">Old object (the one currently saved in db). Available only for update event types</param>
        /// <returns>New obj with updated data</returns>
        public virtual void ExtendObject(SaveEventType eventType, TEntity newObj, TEntity oldObj = null)
        {
            // do not change anything by default
        }

        #endregion

        #region IService members

        /// <summary>
        /// Touches given cache key in order to clear the item
        /// </summary>
        public virtual void TouchKey(string key)
        {
            CacheService.TouchKey(key);
        }

        /// <summary>
        /// Touches all keys for insert action
        /// </summary>
        public virtual void TouchInsertKeys(TEntity obj)
        {
            CacheService.TouchKey(EntityKeys.KeyCreateAny<TEntity>());
        }

        /// <summary>
        /// Touches all keys for update actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchUpdateKeys(TEntity obj)
        {
            CacheService.TouchKey(EntityKeys.KeyUpdateAny<TEntity>());
            CacheService.TouchKey(EntityKeys.KeyUpdate<TEntity>(obj.GetObjectID().ToString()));
            CacheService.TouchKey(EntityKeys.KeyUpdateCodeName<TEntity>(obj.GetCodeName()));
        }


        /// <summary>
        /// Touches all keys for delete actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchDeleteKeys(TEntity obj)
        {
            CacheService.TouchKey(EntityKeys.KeyDeleteAny<TEntity>());
            CacheService.TouchKey(EntityKeys.KeyDelete<TEntity>(obj.GetObjectID().ToString()));
            CacheService.TouchKey(EntityKeys.KeyDeleteCodeName<TEntity>(obj.GetCodeName()));
        }

        /// <summary>
        /// Used for refreshing context may bring better performance when bulk inserting etc. 
        /// USE WITH CAUTION because the old context is lost
        /// </summary>
        /// <param name="appContext"></param>
        public virtual void RefreshAppContext(IAppContext appContext)
        {
            // dispose old context
            Dispose();

            // assign new context
            AppContext = appContext;
        }

        /// <summary>
        /// Deletes entity with given ID
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(int id)
        {
            return DeleteObjectAsync(EntityDbSet, id);
        }

        /// <summary>
        /// Inserts entity into database
        /// </summary>
        /// <param name="obj">Entity to insert</param>
        /// <returns>Result of the insert action</returns>
        public virtual Task<IInsertActionResult> InsertAsync(TEntity obj)
        {
            return InsertObjectAsync(EntityDbSet, obj);
        }

        /// <summary>
        /// Updates given entity in database
        /// </summary>
        /// <param name="obj">Entity to edit</param>
        /// <returns>Number of updated rows</returns>
        public virtual Task<int> UpdateAsync(TEntity obj)
        {
            return UpdateObjectAsync(EntityDbSet, obj);
        }

        /// <summary>
        /// Updates given entity in database
        /// </summary>
        /// <param name="obj">New entity to edit</param>
        /// <param name="existingEntity">Existing entity in db</param>
        /// <returns>Number of updated rows</returns>
        public virtual Task<int> UpdateAsync(TEntity obj, TEntity existingEntity)
        {
            return UpdateObjectAsync(EntityDbSet, obj, existingEntity);
        }

        /// <summary>
        /// Gets single entity
        /// </summary>
        /// <param name="id">EntityID</param>
        /// <returns>Entity</returns>
        public virtual Task<TEntity> GetSingleAsync(int id)
        {
            return EntityDbSet.FirstOrDefaultAsync(m => m.ID == id);
        }

        /// <summary>
        /// Gets query for all entities
        /// </summary>
        /// <returns>Query for all entities</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return EntityDbSet;
        }

        /// <summary>
        /// Gets query for single entity
        /// </summary>
        /// <param name="id">EntityID</param>
        /// <returns>Query for single entity</returns>
        public virtual IQueryable<TEntity> GetSingle(int id)
        {
            return EntityDbSet.Where(m => m.ID == id).Take(1);
        }

        /// <summary>
        /// Gets collection of all entities of current type from cache
        /// </summary>
        /// <returns>Collection of all entities</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }

        #endregion

        #region Events 

        public event EventHandler<InsertEventArgs<TEntity>> OnInsertBeforeObject;
        public event EventHandler<UpdateEventArgs<TEntity>> OnUpdateBeforeObject;
        public event EventHandler<DeleteEventArgs<TEntity>> OnDeleteBeforeObject;

        public event EventHandler<InsertEventArgs<TEntity>> OnInsertAfterObject;
        public event EventHandler<UpdateEventArgs<TEntity>> OnUpdateAfterObject;
        public event EventHandler<DeleteEventArgs<TEntity>> OnDeleteAfterObject;

        /// <summary>
        /// Insert before event action
        /// </summary>
        private void OnInserBefore(TEntity obj)
        {
            var args = new InsertEventArgs<TEntity>()
            {
                Obj = obj,
            };
            OnInsertBeforeObject?.Invoke(this, args);
        }

        /// <summary>
        /// Update before event action
        /// </summary>
        private void OnUpdateBefore(TEntity obj, TEntity originalObj)
        {
            var args = new UpdateEventArgs<TEntity>()
            {
                Obj = obj,
                OriginalObj = originalObj
            };
            OnUpdateBeforeObject?.Invoke(this, args);
        }

        /// <summary>
        /// Delete before event action
        /// </summary>
        private void OnDeleteBefore(TEntity obj)
        {
            var args = new DeleteEventArgs<TEntity>()
            {
                Obj = obj,
            };
            OnDeleteBeforeObject?.Invoke(this, args);
        }

        /// <summary>
        /// Insert after event action
        /// </summary>
        private void OnInsertAfter(TEntity obj)
        {
            var args = new InsertEventArgs<TEntity>()
            {
                Obj = obj,
            };
            OnInsertAfterObject?.Invoke(this, args);
        }

        /// <summary>
        /// Update after event action
        /// </summary>
        private void OnUpdateAfter(TEntity obj, TEntity originalObj)
        {
            var args = new UpdateEventArgs<TEntity>()
            {
                Obj = obj,
                OriginalObj = originalObj
            };
            OnUpdateAfterObject?.Invoke(this, args);
        }

        /// <summary>
        /// Delete after event action
        /// </summary>
        private void OnDeleteAfter(TEntity obj)
        {
            var args = new DeleteEventArgs<TEntity>()
            {
                Obj = obj,
            };
            OnDeleteAfterObject?.Invoke(this, args);
        }

        #endregion

        #region Security checks

        /// <summary>
        /// Checks whether current user can update given entity
        /// </summary>
        /// <param name="obj">Entity to be updated</param>
        /// <returns>True if entity can be updated, false otherwise</returns>
        public virtual bool CanUpdate(TEntity obj)
        {
            // return true immediately if context allows it
            if (!ServiceContext.CheckPermissions)
            {
                return true;
            }

            if (!ServiceDependencies.User.IsAuthenticated)
            {
                // not logged users cannot update entities by default
                return false;
            }

            var entityAccess = obj as IEntityWithRestrictedAccess;

            if (entityAccess == null)
            {
                // entities not marked with restricted access can be updated by default
                return true;
            }

            if (string.IsNullOrEmpty(entityAccess.CreatedByApplicationUserId) || string.IsNullOrEmpty(entityAccess.UpdatedByApplicationUserId))
            {
                throw new NotSupportedException($"Cannot check security because properties of '{nameof(IEntityWithRestrictedAccess)}' class are empty");
            }

            // return true if current user created the entity
            return entityAccess.CreatedByApplicationUserId.Equals(ServiceDependencies.User?.UserId ?? string.Empty);
        }

        /// <summary>
        /// Checks whether current user can delete given entity
        /// </summary>
        /// <param name="obj">Entity to be deleted</param>
        /// <returns>True if entity can be deleted, false otherwise</returns>
        public virtual bool CanDelete(TEntity obj)
        {
            return CanUpdate(obj);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Handles code name of the entity
        /// </summary>
        /// <param name="newEntity">New entity</param>
        private async Task HandleEntityCodeNameAsync(TEntity newEntity)
        {
            var entityWithoutCodeName = newEntity as IEntityWithoutCodeName;
            if (entityWithoutCodeName != null)
            {
                // do not process entities without code name
                return;
            }

            // set entity code name
            newEntity.CodeName = newEntity.GetCodeName();

            // fix empty code name
            if (string.IsNullOrEmpty(newEntity.CodeName))
            {
                throw new InvalidCodeNameException($"Code name cannot be null");
            }

            // check unique code name
            var entityWithUniqueCodeName = newEntity as IEntityWithUniqueCodeName;
            if (entityWithUniqueCodeName != null)
            {
                var result = await EntityDbSet.Select(m => new { m.ID, m.CodeName }).FirstOrDefaultAsync(m => m.Equals(newEntity.CodeName) && m.ID != newEntity.ID);

                if (result != null)
                {
                    // there is already object with the same code name
                    throw new CodeNameNotUniqueException($"Name '{newEntity.CodeName}' is not unique.");
                }
            }
        }

        /// <summary>
        /// Handles code name of the entity
        /// </summary>
        /// <param name="newEntity">New entity</param>
        private void HandleEntityCodeName(TEntity newEntity)
        {
            var entityWithoutCodeName = newEntity as IEntityWithoutCodeName;
            if (entityWithoutCodeName != null)
            {
                // do not process entities without code name
                return;
            }

            // set entity code name
            newEntity.CodeName = newEntity.GetCodeName();

            // fix empty code name
            if (string.IsNullOrEmpty(newEntity.CodeName))
            {
                throw new InvalidCodeNameException($"Code name cannot be null");
            }

            // check unique code name
            var entityWithUniqueCodeName = newEntity as IEntityWithUniqueCodeName;
            if (entityWithUniqueCodeName != null)
            {
                var result = EntityDbSet.Select(m => new { m.ID, m.CodeName }).FirstOrDefault(m => m.Equals(newEntity.CodeName) && m.ID != newEntity.ID);

                if (result != null)
                {
                    // there is already object with the same code name
                    throw new CodeNameNotUniqueException($"Name '{newEntity.CodeName}' is not unique.");
                }
            }
        }

        /// <summary>
        /// Sets entity specific fields (e.g. guid, created, updated etc..)
        /// </summary>
        /// <param name="type">Save event type</param>
        /// <param name="oldEntity">Existing entity in db</param>
        /// <param name="newEntity">New entity</param>
        private void HandleEntityVariationFields(SaveEventType type, TEntity newEntity, TEntity oldEntity)
        {
            var entityWithGuid = newEntity as IEntityWithGuid;
            if (entityWithGuid != null)
            {
                if (type == SaveEventType.Insert)
                {
                    // set new guid
                    entityWithGuid.Guid = Guid.NewGuid();
                }
                else if (type == SaveEventType.Update)
                {
                    var oldEntityWithGuid = oldEntity as IEntityWithGuid;
                    if (oldEntityWithGuid == null)
                    {
                        throw new NotSupportedException($"Cannot set GUID for '{nameof(oldEntity)}' entity");
                    }
                    // keep the old guid
                    entityWithGuid.Guid = oldEntityWithGuid.Guid;
                }
            }

            var entityWithTimeStamp = newEntity as IEntityWithTimeStamp;
            if (entityWithTimeStamp != null)
            {
                if (type == SaveEventType.Insert)
                {
                    // set new created time
                    entityWithTimeStamp.Created = DateTime.Now;
                    entityWithTimeStamp.Updated = DateTime.Now;
                }
                else if (type == SaveEventType.Update)
                {
                    entityWithTimeStamp.Updated = DateTime.Now;
                    // keep the old created time
                    var oldEntityWithTimeStamp = oldEntity as IEntityWithTimeStamp;
                    if (oldEntityWithTimeStamp == null)
                    {
                        throw new NotSupportedException($"Cannot set timestamp for '{nameof(oldEntity)}' entity");
                    }
                    entityWithTimeStamp.Created = oldEntityWithTimeStamp.Created;
                }
            }

            var entityWithOptionalUserStamp = newEntity as IEntityWithOptionalUserStamp;
            if (entityWithOptionalUserStamp != null)
            {
                // required user stamp
                if (ServiceDependencies.User.IsAuthenticated)
                {


                    if (type == SaveEventType.Insert)
                    {
                        entityWithOptionalUserStamp.CreatedByApplicationUserId = ServiceDependencies.User.UserId;
                        entityWithOptionalUserStamp.UpdatedByApplicationUserId = ServiceDependencies.User.UserId;
                    }
                    else if (type == SaveEventType.Update)
                    {
                        // set updated value
                        entityWithOptionalUserStamp.UpdatedByApplicationUserId = ServiceDependencies.User.UserId;

                        var oldEntityWithOptionalUserStamp = oldEntity as IEntityWithOptionalUserStamp;
                        if (oldEntityWithOptionalUserStamp == null)
                        {
                            throw new NotSupportedException($"Cannot set user stamp for '{nameof(oldEntity)}' entity");
                        }
                        // keep the created value
                        entityWithOptionalUserStamp.CreatedByApplicationUserId = oldEntityWithOptionalUserStamp.CreatedByApplicationUserId;
                    }
                }
            }

            var entityWithUserStamp = newEntity as IEntityWithUserStamp;
            if (entityWithUserStamp != null)
            {
                // required user stamp
                if (!ServiceDependencies.User.IsAuthenticated)
                {
                    throw new AuthorizationException($"Authentication required for action '{type}'");
                }

                if (type == SaveEventType.Insert)
                {
                    entityWithUserStamp.CreatedByApplicationUserId = ServiceDependencies.User.UserId;
                    entityWithUserStamp.UpdatedByApplicationUserId = ServiceDependencies.User.UserId;
                }
                else if (type == SaveEventType.Update)
                {
                    // set updated value
                    entityWithUserStamp.UpdatedByApplicationUserId = ServiceDependencies.User.UserId;

                    var oldEntityWithUserStamp = oldEntity as IEntityWithUserStamp;
                    if (oldEntityWithUserStamp == null)
                    {
                        throw new NotSupportedException($"Cannot set user stamp for '{nameof(oldEntity)}' entity");
                    }
                    // keep the created value
                    entityWithUserStamp.CreatedByApplicationUserId = oldEntityWithUserStamp.CreatedByApplicationUserId;
                }
            }


            var entityWithActiveState = newEntity as IEntityWithActiveState;
            if (entityWithActiveState != null)
            {
                if (type == SaveEventType.Insert)
                {
                    if (entityWithActiveState.IsActive)
                    {
                        entityWithActiveState.ActiveSince = DateTime.Now;
                    }
                    else
                    {
                        entityWithActiveState.ActiveSince = null;
                    }
                }
                else if (type == SaveEventType.Update)
                {
                    var oldEntityWithActiveState = oldEntity as IEntityWithActiveState;
                    if (oldEntityWithActiveState == null)
                    {
                        throw new NotSupportedException($"Cannot set active state for '{nameof(oldEntity)}' entity");
                    }

                    // set active since to current time if entity was not active before, but is active now
                    if (!oldEntityWithActiveState.IsActive && entityWithActiveState.IsActive)
                    {
                        entityWithActiveState.ActiveSince = DateTime.Now;
                    }
                    // set active since to min Time if entity was active, but is not anymore
                    else if (oldEntityWithActiveState.IsActive && !entityWithActiveState.IsActive)
                    {
                        entityWithActiveState.ActiveSince = null;
                    }
                    else
                    {
                        // keep the active since if state didn't change
                        entityWithActiveState.ActiveSince = oldEntityWithActiveState.ActiveSince;
                    }
                }
            }
        }

        /// <summary>
        /// Gets cache setup used to save all objects of this type to cache
        /// </summary>
        /// <returns>Cache setup</returns>
        private ICacheSetup GetCacheAllCacheSetup()
        {
            const int cacheMinutes = 120;
            const string cacheKey = nameof(GetCacheAllCacheSetup);

            var cacheSetup = CacheService.GetSetup<TEntity>(cacheKey, cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<TEntity>(),
                EntityKeys.KeyDeleteAny<TEntity>(),
                EntityKeys.KeyCreateAny<TEntity>(),
            };
            cacheSetup.ObjectStringID = GetType().Name;

            return cacheSetup;
        }


        private string GetSaveExceptionText(SaveEventType type)
        {
            return $"Save failed for action '{type}'";
        }

        #endregion
    }
}
