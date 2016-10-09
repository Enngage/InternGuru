using System;
using System.Threading.Tasks;
using Core.Context;
using Cache;
using Entity;
using Core.Events;
using System.Collections.Generic;

namespace Core.Services
{
    public class BaseService<T> : IDisposable where T : class, IEntity
    {
        #region Variables

        private IAppContext appContext;
        private ICacheService cacheService;
        private ILogService logService;

        #endregion

        #region Properties

        /// <summary>
        /// AppContext 
        /// </summary>
        protected IAppContext AppContext
        {
            get
            {
                return this.appContext;
            }
        }

        /// <summary>
        /// Cache service
        /// </summary>
        protected ICacheService CacheService
        {
            get
            {
                return this.cacheService;
            }
        }

        /// <summary>
        /// Log service
        /// </summary>
        protected ILogService LogService
        {
            get
            {
                return this.logService;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes service abstract instance
        /// </summary>
        /// <param name="appContext">AppContext</param>
        /// <param name="cacheService">Cache service</param>
        /// <param name="logService">Log service (can be null because of cyclic ILogService)</param>
        public BaseService(IAppContext appContext, ICacheService cacheService, ILogService logService = null)
        {
            this.appContext = appContext;
            this.cacheService = cacheService;
            this.logService = logService;
        }

        #endregion

        #region IDispose members

        public void Dispose()
        {
            if (this.appContext != null)
            {
                this.appContext.Dispose();
            }
        }

        #endregion

        #region Save methods

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        protected int SaveChanges()
        {
            try
            {
                return this.appContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // log event
                if (logService != null)
                {
                    logService.LogException(ex);
                }

                // re-throw
                throw;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        protected Task<int> SaveChangesAsync()
        {
            try
            {
                return this.appContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // log event
                if (logService != null)
                {
                    logService.LogException(ex);
                }

                // re-throw
                throw;
            }
        }

        #endregion

        #region Events 

        public event EventHandler<InsertEventArgs<T>> OnInsertObject;
        public event EventHandler<UpdateEventArgs<T>> OnUpdateObject;
        public event EventHandler<DeleteEventArgs<T>> OnDeleteObject;

        /// <summary>
        /// Insert event action
        /// </summary>
        /// <param name="e"></param>
        protected void OnInsert(T obj)
        {
            var args = new InsertEventArgs<T>()
            {
                Obj = obj
            };
            OnInsertObject?.Invoke(this, args);
        }

        /// <summary>
        /// Update event action
        /// </summary>
        /// <param name="e"></param>
        protected void OnUpdate(T obj)
        {
            var args = new UpdateEventArgs<T>()
            {
                Obj = obj
            };
            OnUpdateObject?.Invoke(this, args);
        }

        /// <summary>
        /// Delete event action
        /// </summary>
        /// <param name="e"></param>
        protected void OnDelete(T obj)
        {
            var args = new DeleteEventArgs<T>()
            {
                Obj = obj
            };
            OnDeleteObject?.Invoke(this, args);
        }

        #endregion

        #region IService

        /// <summary>
        /// Touches given cache key in order to clear the item
        /// </summary>
        public virtual void TouchKey(string key)
        {
            cacheService.TouchKey(key);
        }

        /// <summary>
        /// Touches all keys for insert action
        /// </summary>
        public virtual void TouchInsertKeys(T obj)
        {
            cacheService.TouchKey(EntityKeys.KeyCreateAny<T>());
        }

        /// <summary>
        /// Touches all keys for update actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchUpdateKeys(T obj)
        {
            cacheService.TouchKey(EntityKeys.KeyUpdateAny<T>());
            cacheService.TouchKey(EntityKeys.KeyUpdateCodeName<T>(obj.GetCodeName()));
            cacheService.TouchKey(EntityKeys.KeyUpdate<T>(obj.GetObjectID().ToString()));
        }


        /// <summary>
        /// Touches all keys for delete actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchDeleteKeys(T obj)
        {
            cacheService.TouchKey(EntityKeys.KeyDeleteAny<T>());
            cacheService.TouchKey(EntityKeys.KeyDeleteCodeName<T>(obj.GetCodeName()));
            cacheService.TouchKey(EntityKeys.KeyDelete<T>(obj.GetObjectID().ToString()));
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
            this.appContext = appContext;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets cache setup used to save all objects of this type to cache
        /// </summary>
        /// <returns>Cache setup</returns>
        protected ICacheSetup GetCacheAllCacheSetup()
        {
            int cacheMinutes = 120;
            var cacheKey = $"GetCacheAllCacheSetup";

            var cacheSetup = CacheService.GetSetup<T>(cacheKey, cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<T>(),
                EntityKeys.KeyDeleteAny<T>(),
                EntityKeys.KeyCreateAny<T>(),
            };
            cacheSetup.ObjectStringID = this.GetType().Name;

            return cacheSetup;
        }

        #endregion
    }
}
