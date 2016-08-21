using System;
using System.Threading.Tasks;
using Core.Context;
using Cache;

namespace Core.Services
{
    public class ServiceAbstract : IDisposable
    {
        #region Variables

        private IAppContext appContext;
        private ICacheService cacheService;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes service abstract instance
        /// </summary>
        /// <param name="appContext">AppContext</param>
        /// <param name="cacheService">Cache service</param>
        public ServiceAbstract(IAppContext appContext, ICacheService cacheService)
        {
            this.appContext = appContext;
            this.cacheService = cacheService;
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

        protected void SaveChanges()
        {
            try
            {
                this.appContext.SaveChanges();
            }
            catch
            {
                // re-throw
                throw;
            }
        }

        protected async Task SaveChangesAsync()
        {
            try
            {
                await this.appContext.SaveChangesAsync();
            }
            catch
            {
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
