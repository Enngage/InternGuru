using System;
using System.Threading.Tasks;
using Service.Context;
using Cache;
using Service.Events;
using System.Collections.Generic;
using Core.Loc;
using EmailProvider;
using Entity.Base;
using Service.Services.Logs;

namespace Service.Services
{
    public class BaseService<T> : IDisposable where T : class, IEntity
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

        #region Save methods

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        protected int SaveChanges()
        {
            try
            {
                return AppContext.SaveChanges();
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

        /// <summary>
        /// Saves all changes made in this context to the underlying database
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        protected Task<int> SaveChangesAsync()
        {
            try
            {
                return AppContext.SaveChangesAsync();
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

        public event EventHandler<InsertEventArgs<T>> OnInsertObject;
        public event EventHandler<UpdateEventArgs<T>> OnUpdateObject;
        public event EventHandler<DeleteEventArgs<T>> OnDeleteObject;

        /// <summary>
        /// Insert event action
        /// </summary>
        protected void OnInsert(T obj)
        {
            var args = new InsertEventArgs<T>()
            {
                Obj = obj,
            };
            OnInsertObject?.Invoke(this, args);
        }

        /// <summary>
        /// Update event action
        /// </summary>
        protected void OnUpdate(T obj, T originalObj)
        {
            var args = new UpdateEventArgs<T>()
            {
                Obj = obj,
                OriginalObj = originalObj
            };
            OnUpdateObject?.Invoke(this, args);
        }

        /// <summary>
        /// Delete event action
        /// </summary>
        protected void OnDelete(T obj)
        {
            var args = new DeleteEventArgs<T>()
            {
                Obj = obj,
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
            CacheService.TouchKey(key);
        }

        /// <summary>
        /// Touches all keys for insert action
        /// </summary>
        public virtual void TouchInsertKeys(T obj)
        {
            CacheService.TouchKey(EntityKeys.KeyCreateAny<T>());
        }

        /// <summary>
        /// Touches all keys for update actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchUpdateKeys(T obj)
        {
            CacheService.TouchKey(EntityKeys.KeyUpdateAny<T>());
            CacheService.TouchKey(EntityKeys.KeyUpdateCodeName<T>(obj.GetCodeName()));
            CacheService.TouchKey(EntityKeys.KeyUpdate<T>(obj.GetObjectID().ToString()));
        }


        /// <summary>
        /// Touches all keys for delete actions
        /// </summary>
        /// <param name="obj">Object</param>
        public virtual void TouchDeleteKeys(T obj)
        {
            CacheService.TouchKey(EntityKeys.KeyDeleteAny<T>());
            CacheService.TouchKey(EntityKeys.KeyDeleteCodeName<T>(obj.GetCodeName()));
            CacheService.TouchKey(EntityKeys.KeyDelete<T>(obj.GetObjectID().ToString()));
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

            var cacheSetup = CacheService.GetSetup<T>(cacheKey, cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<T>(),
                EntityKeys.KeyDeleteAny<T>(),
                EntityKeys.KeyCreateAny<T>(),
            };
            cacheSetup.ObjectStringID = GetType().Name;

            return cacheSetup;
        }

        #endregion
    }
}
