using System;
using System.Runtime.CompilerServices;
using System.Linq;

using Core.Context;
using System.Web;
using Cache;
using Core.Services;
using Core.Services.Identity.Models;
using UI.ModelState;

namespace UI.Abstract
{
    public abstract class BuilderAbstract : IDisposable
    {

        #region Variables

        private IAppContext appContext;
        private ICurrentUser currentUser;
        private ICacheService cacheService;
        private IIdentityService identityService;
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
        /// LogService 
        /// </summary>
        protected ILogService LogService
        {
            get
            {
                return this.logService;
            }
        }

        /// <summary>
        /// Current user
        /// </summary>
        public ICurrentUser CurrentUser
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
        /// Identity service 
        /// </summary>
        protected IIdentityService IdentityService
        {
            get
            {
                return this.identityService;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes builder
        /// </summary>
        /// <param name="appContext">appContext</param>
        /// <param name="cacheService">cacheService</param>
        /// <param name="identityService">identityService</param>
        /// <param name="logService">logService</param>
        public BuilderAbstract(
            IAppContext appContext, 
            ICacheService cacheService,
            IIdentityService identityService,
            ILogService logService)
        {
            this.appContext = appContext;
            this.cacheService = cacheService;
            this.identityService = identityService;
            this.logService = logService;

            // Initialize current user
            InitializeCurrentUser();
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

        /// <summary>
        /// Initializes current user
        /// </summary>
        private void InitializeCurrentUser()
        {
            this.currentUser = GetCurrentuser();

        }

        private ICurrentUser GetCurrentuser()
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
                            // try to get user id from db
                            int cacheMinutes = 60;
                            var cacheSetup = this.cacheService.GetSetup<ICurrentUser>("BuilderAbstract.GetCurrentUser", cacheMinutes);
                            var userId = this.cacheService.GetOrSet<string>(() => GetApplicationUserId(currentIdentity.Name), cacheSetup);

                            return new CurrentUser()
                            {
                                Name = currentIdentity.Name,
                                AuthenticationType = currentIdentity.AuthenticationType,
                                Id = userId,
                                IsAuthenticated = currentIdentity.IsAuthenticated
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets application user id of user
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <returns>ApplicationUserId</returns>
        private string GetApplicationUserId(string userName)
        {
            var userID = this.IdentityService.GetSingle(userName)
                .Select(m => m.Id)
                .FirstOrDefault();

            return userID;
        }

        #endregion
    }
}
