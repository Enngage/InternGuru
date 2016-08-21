using System;
using System.Runtime.CompilerServices;

using Core.Context;
using System.Security.Principal;
using System.Web;
using Cache;

namespace UI.Abstract
{
    public abstract class BuilderAbstract : IDisposable
    {

        #region Variables

        private IAppContext appContext;
        private IIdentity currentUser;
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
        /// Current user
        /// </summary>
        protected IIdentity CurrentUser
        {
            get
            {
                return this.currentUser;
            }
        }

        /// <summary>
        /// Indicates if user is authenticated
        /// </summary>
        protected bool IsAuthenticated
        {
            get
            {
                return CurrentUser == null;
            }
        }

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
        /// Initializes builder
        /// </summary>
        /// <param name="appContext">appContext</param>
        /// <param name="cacheService">cacheService</param>
        public BuilderAbstract(IAppContext appContext, ICacheService cacheService)
        {
            this.appContext = appContext;
            this.cacheService = cacheService;
            this.currentUser = GetCurrentuser();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes App context
        /// </summary>
        public void Dispose()
        {
            if (this.appContext != null)
            {
                this.Dispose();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets name of method of current method
        /// </summary>
        protected string GetSource([CallerMemberName]string memberName = "")
        {
            return memberName; //output will me name of calling method
        }

        private IIdentity GetCurrentuser()
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
                            return currentIdentity;
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
