using System;
using System.Runtime.CompilerServices;
using System.Linq;

using Service.Context;
using System.Web;
using System.Collections.Generic;
using Entity;
using UI.Builders.Services;
using Core.Helpers.Privilege;
using Microsoft.AspNet.Identity;
using UI.Builders.Shared.Models;
using Identity;
using Core.Config;
using Entity.Base;
using UI.Builders.Shared.Enums;
using UI.Modules.Header;

namespace UI.Base
{
    public abstract class BaseBuilder : IDisposable
    {

        #region Variables

        private readonly ApplicationUserManager _userManager;

        #endregion

        #region Properties

        /// <summary>
        /// AppContext 
        /// </summary>
        protected IAppContext AppContext { get; }

        /// <summary>
        /// Header
        /// </summary>
        public IUiHeader UiHeader { get; private set; }

        /// <summary>
        /// Current user
        /// </summary>
        public ICurrentUser CurrentUser { get; private set; }

        /// <summary>
        /// Company of current user
        /// </summary>
        public ICurrentCompany CurrentCompany { get; private set; }


        /// <summary>
        /// Status box of current user
        /// </summary>
        public IStatusBox StatusBox { get; private set; }


        /// <summary>
        /// List of all available services
        /// </summary>
        protected IServicesLoader Services { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes builder
        /// </summary>
        /// <param name="systemContext">systemContext</param>
        /// <param name="servicesLoader">services loader used to initialize all services</param>
        protected BaseBuilder(ISystemContext systemContext, IServicesLoader servicesLoader)
        {
            AppContext = systemContext.AppContext;
            Services = servicesLoader;
            _userManager = systemContext.ApplicationUserManager;

            // Initialize current user
            InitializeCurrentUser();

            // Initialize company of current user if user is authenticated          
            InitializeCurrentCompany(CurrentUser.Id);

            // Initialize status box
             InitializeStatusBox(CurrentUser.Id);

            // Initialize header
            InitializeUiHeader();

        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes App context
        /// </summary>
        public void Dispose()
        {
            if (AppContext != null)
            {
                Dispose();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets name of method of current method
        /// </summary>
        protected string GetSource([CallerMemberName]string memberName = "")
        {
            return memberName; //output will me name of calling method
        }

        /// <summary>
        /// Re-initializes company of current user
        /// Call when current company is changed/deleted/created
        /// </summary>
        /// <param name="applicationUserId">applicationUserId</param>
        protected void ReInitializeCurrentCompany(string applicationUserId)
        {
            InitializeCurrentCompany(applicationUserId);
        }

        /// <summary>
        /// Initializes current user
        /// </summary>
        private void InitializeCurrentUser()
        {
            CurrentUser = GetCurrentuser();

        }

        /// <summary>
        /// Initializes status box
        /// </summary>
        /// <param name="applicationUserId">UserId</param>
        private void InitializeStatusBox(string applicationUserId)
        {
            if (CurrentUser.IsAuthenticated) { 
                var statusBox = new StatusBox()
                {
                    NewMessagesCount = GetNumberOfNewMessages(applicationUserId),
                    NewEventLogCount = GetNumberOfNewLogs()
                };

            StatusBox = statusBox;
            }
        }

        /// <summary>
        /// Initializes header
        /// </summary>
        private void InitializeUiHeader()
        {
            UiHeader = new UiHeader()
            {
                Type = UiHeaderType.None,
                Title = string.Empty
            };
        }

        /// <summary>
        /// Initializes current company
        /// </summary>
        /// <param name="applicationUserId">applicationUserId</param>
        private void InitializeCurrentCompany(string applicationUserId)
        {

            if (CurrentUser.IsAuthenticated)
            {
                CurrentCompany = GetCompanyOfUser(applicationUserId);
            }
        }

        private ICurrentUser GetCurrentuser()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.User != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var currentIdentity = HttpContext.Current.User.Identity;

                        if (currentIdentity != null)
                        {
                            // ----- Process current user ------- //
                            var cacheKey = "BuilderAbstract.GetCurrentUser";
                            var cacheMinutes = 120;
                            var cacheSetup = Services.CacheService.GetSetup<ICurrentUser>(cacheKey, cacheMinutes);
                            cacheSetup.Dependencies = new List<string>()
                            {
                                EntityKeys.KeyUpdate<ApplicationUser>(currentIdentity.Name),
                                EntityKeys.KeyUpdateAny<ApplicationUser>()
                            };
                            cacheSetup.ObjectStringID = currentIdentity.Name;

                            var user = Services.CacheService.GetOrSet(() => GetApplicationUser(currentIdentity.Name, currentIdentity.AuthenticationType), cacheSetup);

                            if (user == null)
                            {
                                // user was not found in DB
                                return new CurrentUser()
                                {
                                    IsAuthenticated = false,
                                    Privilege = PrivilegeLevel.Public
                                };
                            }

                            return user;
                        }
                    }
                }
            }

            // user is not authenticated if we got this far
            return new CurrentUser()
            {
                IsAuthenticated = false,
                Privilege = PrivilegeLevel.Public
            };
        }

        /// <summary>
        /// Gets application user 
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <returns>ApplicationUser</returns>
        private ICurrentUser GetApplicationUser(string userName, string authenticationType)
        {
            var user = Services.IdentityService.GetAll()
                .Where(m => m.UserName == userName)
                .Select(m => new CurrentUser()
                {
                    UserName = m.UserName,
                    Id = m.Id,
                    AuthenticationType = authenticationType,
                    IsAuthenticated = true,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    Nickname = m.Nickname
                })
                .FirstOrDefault();
            
            if (user == null)
            {
                return null;
            }

            // get roles of user
            user.Roles = _userManager.GetRoles(user.Id);

            // set privilege level
            user.Privilege = PrivilegeHelper.GetPrivilegeLevel(user.Roles, PrivilegeLevel.Authenticated);

            return user;
        }


        /// <summary>
        /// Gets company of current user or null if it doesnt exist
        /// </summary>
        /// <param name="applicationUserId">applicationUserId</param>
        /// <returns>Company of current user or null if not found</returns>
        private ICurrentCompany GetCompanyOfUserInternal(string applicationUserId)
        {
            var company = Services.CompanyService.GetAll()
                .Where(m => m.ApplicationUserId == applicationUserId)
                .Select(m => new CurrentCompany()
                {
                    CompanyID = m.ID,
                    CompanyName = m.CompanyName,
                    CompanyCreatedByApplicationUserId = m.ApplicationUser.Id,
                    CompanyCreatedByApplicationUserName = m.ApplicationUser.UserName,
                    CompanyGuid = m.CompanyGuid
                })
                .FirstOrDefault();

            return company;
        }

        /// <summary>
        /// Gets company of current user from cache
        /// Empty company is used if user hasnt created any company yet
        /// </summary>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns>Company of current user (emnpty object is returned if company does not exist)</returns>
        private ICurrentCompany GetCompanyOfUser(string applicationUserId)
        {
            var cacheSetup = Services.CacheService.GetSetup<ICurrentCompany>(GetSource());
            cacheSetup.Dependencies = new List<string>()
                            {
                                EntityKeys.KeyDeleteAny<Company>(),
                                EntityKeys.KeyCreateAny<Company>(),
                                EntityKeys.KeyUpdateAny<Company>(),
                                EntityKeys.KeyUpdate<ApplicationUser>(applicationUserId),
                            };
            cacheSetup.ObjectStringID = applicationUserId; // identify company of user based on UserName

            var company = Services.CacheService.GetOrSet(() => GetCompanyOfUserInternal(applicationUserId), cacheSetup);

            return company ?? new CurrentCompany();
        }

        /// <summary>
        /// Gets status box from cache
        /// </summary>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns>Status box</returns>
        private int GetNumberOfNewMessages(string applicationUserId)
        {
            var cacheSetup = Services.CacheService.GetSetup<IStatusBox>(GetSource());
            cacheSetup.Dependencies = new List<string>()
                            {
                                EntityKeys.KeyUpdateAny<Message>(),
                                EntityKeys.KeyDeleteAny<Message>(),
                                EntityKeys.KeyCreateAny<Message>(),
                            };
            cacheSetup.ObjectStringID = applicationUserId;

            var newMessages = Services.CacheService.GetOrSet(() => GetNumberOfNewMessagesInternal(applicationUserId), cacheSetup);

            return newMessages.Value;
        }

        /// <summary>
        /// Gets number of new messages from cache
        /// </summary>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns></returns>
        private IntWrapper GetNumberOfNewMessagesInternal(string applicationUserId)
        {
            // ---------- messages -------- //
            var newMessagesQuery = Services.MessageService.GetAll()
                .Where(m => m.RecipientApplicationUserId == applicationUserId && !m.IsRead)
                .Select(m => new
                {
                    m.ID
                });

            return new IntWrapper()
            {
                Value = newMessagesQuery.Count()
            };
        }

        #endregion

        #region Log helper methods

        private int GetNumberOfNewLogs()
        {
            // only admins can access log
            if (CurrentUser.Privilege != PrivilegeLevel.Admin)
            {
                return 0;
            }

            var latestReadLogIDCookieName = AppConfig.CookieNames.LatestReadLogID;

            var latestReadLogID = Services.CookieService.GetCookieValue(latestReadLogIDCookieName);
            if (string.IsNullOrEmpty(latestReadLogID))
            {
                // cookie was not yet initialized, get all events
                return (GetNumberOfEventsByThreshold(0));
            }

            return (GetNumberOfEventsByThreshold(Convert.ToInt32(latestReadLogID)));
        }

        private int GetNumberOfEventsByThreshold(int logIDThreshold)
        {
            var cacheMinutes = 1; // cache only for 1 minute
            var cacheSetup = Services.CacheService.GetSetup<IntWrapper>(GetSource(), cacheMinutes);
            cacheSetup.ObjectID = logIDThreshold;

            var eventsQuery = Services.LogService.GetAll()
               .Where(m => m.ID > logIDThreshold)
               .Select(m => new IntWrapper()
               {
                   Value = m.ID
               })
               .OrderByDescending(m => m);

            var events = Services.CacheService.GetOrSet(() => eventsQuery.ToList(), cacheSetup);

            return events.Count;
        }

        #endregion
    }
}
