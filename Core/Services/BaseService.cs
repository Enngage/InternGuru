using System;
using System.Threading.Tasks;
using Core.Context;
using Cache;
using Entity;

namespace Core.Services
{
    public class BaseService<T> : IDisposable where T : EntityAbstract
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

        #region IService members

        /// <summary>
        /// Touches given cache key in order to clear the item
        /// </summary>
        public void TouchKey(string key)
        {
            cacheService.TouchKey(key);
        }

        /// <summary>
        /// Touches all keys for insert action
        /// </summary>
        public void TouchInsertKeys(T obj)
        {
            cacheService.TouchKey(EntityAbstract.KeyCreateAny<T>());
        }

        /// <summary>
        /// Touches all keys for update actions
        /// </summary>
        /// <param name="obj">Object</param>
        public void TouchUpdateKeys(T obj)
        {
            cacheService.TouchKey(EntityAbstract.KeyUpdateAny<T>());
            cacheService.TouchKey(EntityAbstract.KeyUpdateCodeName<T>(obj.GetCodeName()));
            cacheService.TouchKey(EntityAbstract.KeyUpdate<T>(obj.GetObjectID().ToString()));
        }


        /// <summary>
        /// Touches all keys for delete actions
        /// </summary>
        /// <param name="obj">Object</param>
        public void TouchDeleteKeys(T obj)
        {
            cacheService.TouchKey(EntityAbstract.KeyDeleteAny<T>());
            cacheService.TouchKey(EntityAbstract.KeyDeleteCodeName<T>(obj.GetCodeName()));
            cacheService.TouchKey(EntityAbstract.KeyDelete<T>(obj.GetObjectID().ToString()));
        }

        /// <summary>
        /// Used for refreshing context may bring better performance when bulk inserting etc. 
        /// USE WITH CAUTION because the old context is lost
        /// </summary>
        /// <param name="appContext"></param>
        public void RefreshAppContext(IAppContext appContext)
        {
            // dispose old context
            Dispose();

            // assign new context
            this.appContext = appContext;
        }

        #endregion
    }
}
