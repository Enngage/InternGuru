using UI.Abstract;
using Core.Context;
using Cache;
using System.Linq;
using UI.Builders.Master.Models;
using System.Web;

namespace UI.Builders.Master
{
    public class MasterBuilder : BuilderAbstract
    {

        #region Services

        #endregion

        #region Constructor

        public MasterBuilder(
            IAppContext appContext,
            ICacheService cacheService
            ) : base(appContext, cacheService)
        {
        }

        #endregion

        #region Methods

        public MasterModel GetMasterModel()
        {
            int cacheMinutes = 30;

            var cacheSetup = CacheService.GetSetup<MasterModel>(GetMasterModelCacheKey(), cacheMinutes);

            return CacheService.GetOrSet<MasterModel>(() => GetMasterModelInternal(), cacheSetup);
        }

        private MasterModel GetMasterModelInternal()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated != false)
                    {
                        // user is authenticated
                        var currentIdentity = HttpContext.Current.User.Identity;

                        if (currentIdentity != null)
                        {
                            var currentUser = this.AppContext.Users.Where(m => m.UserName == currentIdentity.Name).FirstOrDefault();

                            if (currentUser != null)
                            {
                                return new MasterModel()
                                {
                                    AuthenticatedUserName = currentUser.UserName,
                                    IsAuthenticated = true,
                                    AuthenticatedUserId = currentUser.Id,
                                };
                            }
                        }
                    }
                }
            }

            return new MasterModel()
            {
                AuthenticatedUserName = null,
                IsAuthenticated = false,
                IsAdmin = false
            };
        }

        /// <summary>
        /// Gets cache key using the current user
        /// </summary>
        /// <returns></returns>
        private string GetMasterModelCacheKey()
        {
            return "MasterBuilder.GetMasterModelCacheKey" + GetCurrentUserName();
        }

        /// <summary>
        /// Gets current user name from Identity. Null if user is not authenticated
        /// </summary>
        /// <returns>UserName or null if anonymous visitor</returns>
        private string GetCurrentUserName()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated != false)
                    {
                        var currentIdentity = HttpContext.Current.User.Identity;

                        if (currentIdentity != null)
                        {
                            return currentIdentity.Name;
                        }
                    }
                }
            }

            return null;
        }

        #endregion

    }
}
