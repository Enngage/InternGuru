using System;
using System.Threading.Tasks;
using Service.Context;
using Cache;
using Service.Events;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using Core.Loc;
using EmailProvider;
using Entity.Base;
using Service.Exceptions;
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

        #region Sync Save methods

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected  int SaveChanges()
        {
            return SaveChangesAsync(SaveEventType.Unknown).Result;
        }

        /// <summary>
        /// Updates given object in database
        /// </summary>
        /// <param name="obj">Object to be modified</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int UpdateObject(IDbSet<TEntity> dbSet, TEntity obj)
        {
            return UpdateObjectAsync(dbSet, obj).Result;
        }

        /// <summary>
        /// Updates given object in database
        /// </summary>
        /// <param name="obj">Object to be modified</param>
        /// <param name="existingObj">Existing object in database</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int UpdateObject(IDbSet<TEntity> dbSet, TEntity obj, TEntity existingObj)
        {
            return UpdateObjectAsync(dbSet, obj, existingObj).Result;
        }

        /// <summary>
        /// Insert new object to database
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int InsertObject(IDbSet<TEntity> dbSet, TEntity obj)
        {
            return InsertObjectAsync(dbSet, obj).Result;
        }

        /// <summary>
        /// Deletes given object from database
        /// </summary>
        /// <param name="objectId">ID of the object</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int DeleteObject(IDbSet<TEntity> dbSet, int objectId)
        {
            return DeleteObjectAsync(dbSet, objectId).Result;
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
                var result = this.AppContext.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                // get log service
                var logService = KernelProvider.Get<ILogService>();

                var saveException = new SaveException(GetSaveExceptionText(type), ex);

                // log event
                logService?.LogException(saveException);

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
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
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
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> UpdateObjectAsync(IDbSet<TEntity> dbSet, TEntity obj, TEntity existingObj)
        {
            if (existingObj == null)
            {
                throw new ObjectNotFoundException($"Object of '{nameof(IEntity)}' type with ID = '{obj.GetObjectID()}' was not found.");
            }

            // before event
            OnUpdateBefore(obj, existingObj);

            // update existing object with new values
            AppContext.Entry(existingObj).CurrentValues.SetValues(obj);

            // save changes
            var result = await SaveChangesAsync(SaveEventType.Update);

            // after event
            OnUpdateAfter(obj, existingObj);
            //TODO AUTOMAPPER here to clone object
            // touch cache keys
            TouchUpdateKeys(obj);

            return result;
        }

        /// <summary>
        /// Insert new object to database
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> InsertObjectAsync(IDbSet<TEntity> dbSet, TEntity obj)
        {
            // before event
            OnInserBefore(obj);

            // add object to db set
            dbSet.Add(obj);

            // save changes
            var result = await SaveChangesAsync(SaveEventType.Insert);

            // touch cache keys
            TouchInsertKeys(obj);

            // after event
            OnInsertAfter(obj);

            return result;
        }

        /// <summary>
        /// Deletes given object from database
        /// </summary>
        /// <param name="objectId">ID of the object</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> DeleteObjectAsync(IDbSet<TEntity> dbSet, int objectId)
        {
            // get object
            var obj = dbSet.Find(objectId);

            if (obj == null)
            {
                throw new ObjectNotFoundException($"Object of '{nameof(IEntity)}' type with ID = '{objectId}' was not found.");

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
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> SaveChangesAsync(SaveEventType type)
        {
            try
            {
                // save changes
                var result = await this.AppContext.SaveChangesAsync();

                return result;
            }
            catch (Exception ex)
            {
                // get log service
                var logService = KernelProvider.Get<ILogService>();

                var saveException = new SaveException(GetSaveExceptionText(type), ex);

                // log event
                logService?.LogException(saveException);

                // re-throw
                throw;
            }
        }

        #endregion

        #region IService members

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
        /// <returns></returns>
        public virtual Task<int> InsertAsync(TEntity obj)
        {
            return InsertObjectAsync(EntityDbSet, obj);
        }

        /// <summary>
        /// Updates given entity in database
        /// </summary>
        /// <param name="obj">Entity to edit</param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(TEntity obj)
        {
            return UpdateObjectAsync(EntityDbSet, obj);
        }

        /// <summary>
        /// Updates given entity in database
        /// </summary>
        /// <param name="obj">New entity to edit</param>
        /// <param name="existingEntity">Existing entity in db</param>
        /// <returns></returns>
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

        #region IService

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
            CacheService.TouchKey(EntityKeys.KeyUpdateCodeName<TEntity>(obj.GetCodeName()));
            CacheService.TouchKey(EntityKeys.KeyUpdate<TEntity>(obj.GetObjectID().ToString()));
        }


        /// <summary>
        /// Touches all keys for delete actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchDeleteKeys(TEntity obj)
        {
            CacheService.TouchKey(EntityKeys.KeyDeleteAny<TEntity>());
            CacheService.TouchKey(EntityKeys.KeyDeleteCodeName<TEntity>(obj.GetCodeName()));
            CacheService.TouchKey(EntityKeys.KeyDelete<TEntity>(obj.GetObjectID().ToString()));
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

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets cache setup used to save all objects of this type to cache
        /// </summary>
        /// <returns>Cache setup</returns>
        private ICacheSetup GetCacheAllCacheSetup()
        {
            const int cacheMinutes = 120;
            const string cacheKey = "GetCacheAllCacheSetup";

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
