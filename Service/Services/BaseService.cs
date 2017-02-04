using System;
using System.Threading.Tasks;
using Service.Context;
using Cache;
using Service.Events;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using Core.Loc;
using EmailProvider;
using Entity;
using Entity.Base;
using Service.Services.Logs;

namespace Service.Services
{
    public class BaseService<TEntity> : IDisposable where TEntity : class, IEntity
    {
        #region Variables

        /// <summary>
        /// Service dependencies
        /// </summary>
        protected IServiceDependencies ServiceDependencies { get; }

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
        public BaseService(IServiceDependencies serviceDependencies)
        {
            ServiceDependencies = serviceDependencies;
        }

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
        protected int SaveChanges()
        {
            return SaveChanges(SaveEventType.Unknown, null, null);
        }


        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="type">Action type</param>
        /// <param name="entity">New entity</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int SaveChanges(SaveEventType type, TEntity entity)
        {
            return SaveChanges(type, entity, null);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="type">Action type</param>
        /// <param name="entity">New entity</param>
        /// <param name="originalEntity">Original entity</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected int SaveChanges(SaveEventType type, TEntity entity, TEntity originalEntity)
        {
            try
            {
                // before event
                switch (type)
                {
                    case SaveEventType.Delete:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnDeleteBefore(entity);
                        break;
                    case SaveEventType.Insert:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnInserBefore(entity);
                        break;
                    case SaveEventType.Update:
                        // check if original entity was provided
                        if (originalEntity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because original entity was not provided");
                        }
                        OnUpdateAfter(entity, originalEntity);
                        break;
                    case SaveEventType.Unknown:
                        // do nothing
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                // save changes
                var result = this.AppContext.SaveChanges();

                // after event
                switch (type)
                {
                    case SaveEventType.Delete:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnDeleteAfter(entity);
                        break;
                    case SaveEventType.Insert:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnDeleteAfter(entity);
                        break;
                    case SaveEventType.Update:
                        // check if original entity was provided
                        if (originalEntity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because original entity was not provided");
                        }
                        OnUpdateBefore(entity, originalEntity);
                        break;
                    case SaveEventType.Unknown:
                        // do nothing
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                return result;
            }
            catch (Exception ex)
            {
                // get log service
                var logService = KernelProvider.Get<ILogService>();

                // log event
                logService?.LogException(ex);

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
            return await SaveChangesAsync(SaveEventType.Unknown, null, null);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="type">Action type</param>
        /// <param name="entity">New entity</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> SaveChangesAsync(SaveEventType type, TEntity entity)
        {
            return await SaveChangesAsync(type, entity, null);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="objectId">ID of the object</param>
        /// <param name="dbSet">DbSet containing the object</param>
        /// <param name="entity">New entity</param>
        /// <param name="originalEntity">Original entity</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> SaveDeleteChanges(IDbSet<TEntity> dbSet, int objectId)
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


                
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// and executes Entity events
        /// </summary>
        /// <param name="type">Action type</param>
        /// <param name="entity">New entity</param>
        /// <param name="originalEntity">Original entity</param>
        /// <returns> The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).</returns>
        protected async Task<int> SaveChangesAsync(SaveEventType type, TEntity entity, TEntity originalEntity)
        {
            try
            {
                // before event
                switch (type)
                {
                    case SaveEventType.Delete:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnDeleteBefore(entity);
                        break;
                    case SaveEventType.Insert:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnInserBefore(entity);
                        break;
                    case SaveEventType.Update:
                        // check if original entity was provided
                        if (originalEntity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because original entity was not provided");
                        }
                        OnUpdateAfter(entity, originalEntity);
                        break;
                    case SaveEventType.Unknown:
                        // do nothing
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                // save changes
                var result = await this.AppContext.SaveChangesAsync();

                // after event
                switch (type)
                {
                    case SaveEventType.Delete:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnDeleteAfter(entity);
                        break;
                    case SaveEventType.Insert:
                        if (entity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because no entity was not provided");
                        }
                        OnDeleteAfter(entity);
                        break;
                    case SaveEventType.Update:
                        // check if original entity was provided
                        if (originalEntity == null)
                        {
                            throw new NotSupportedException($"Save action '{type}' is not supported because original entity was not provided");
                        }
                        OnUpdateBefore(entity, originalEntity);
                        break;
                    case SaveEventType.Unknown:
                        // do nothing
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                return result;
            }
            catch (Exception ex)
            {
                // get log service
                var logService = KernelProvider.Get<ILogService>();

                // log event
                logService?.LogException(ex);

                // re-throw
                throw;
            }
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
        protected ICacheSetup GetCacheAllCacheSetup()
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

        #endregion
    }
}
