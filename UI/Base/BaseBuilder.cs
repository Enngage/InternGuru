﻿using System;
using System.Runtime.CompilerServices;
using System.Linq;

using Service.Context;
using System.Web;
using System.Collections.Generic;
using Entity;
using UI.Builders.Shared;
using UI.Builders.Services;
using Core.Helpers.Privilege;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using UI.Builders.Shared.Models;
using Identity;
using Core.Config;

namespace UI.Base
{
    public abstract class BaseBuilder : IDisposable
    {

        #region Variables

        private IAppContext appContext;
        private ICurrentUser currentUser;
        private ICurrentCompany currentCompany;
        private IStatusBox statusBox;
        private IUIHeader uiHeader;

        private ApplicationUserManager userManager;
        private ApplicationSignInManager signInManager;

        private IServicesLoader services;

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
        /// Header
        /// </summary>
        public IUIHeader UIHeader
        {
            get
            {
                return this.uiHeader;
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
        /// Company of current user
        /// </summary>
        public ICurrentCompany CurrentCompany
        {
            get
            {
                return this.currentCompany;
            }
        }


        /// <summary>
        /// Status box of current user
        /// </summary>
        public IStatusBox StatusBox
        {
            get
            {
                return this.statusBox;
            }
        }


        /// <summary>
        /// List of all available services
        /// </summary>
        protected IServicesLoader Services
        {
            get
            {
                return this.services;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes builder
        /// </summary>
        /// <param name="systemContext">systemContext</param>
        /// <param name="servicesLoader">services loader used to initialize all services</param>
        public BaseBuilder(ISystemContext systemContext, IServicesLoader servicesLoader)
        {
            this.appContext = systemContext.AppContext;
            this.services = servicesLoader;
            this.userManager = systemContext.ApplicationUserManager;
            this.signInManager = systemContext.ApplicationSignInManager;

            // Initialize current user
            InitializeCurrentUser();

            // Initialize company of current user if user is authenticated
            if (CurrentUser.IsAuthenticated)
            {
                InitializeCurrentCompany(this.CurrentUser.UserName, this.CurrentUser.Id);
            }

            // Initialize status box
            if (CurrentUser.IsAuthenticated)
            {
                InitializeStatusBox(this.CurrentUser.Id);
            }

            // Initialize header
            InitializeUIHeader();

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

        #region Methods

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

        /// <summary>
        /// Initializes status box
        /// </summary>
        /// <param name="applicationUserId">UserId</param>
        private void InitializeStatusBox(string applicationUserId)
        {
            var statusBox = new StatusBox()
            {
                NewMessagesCount = GetNumberOfNewMessages(applicationUserId),
                NewEventLogCount = GetNumberOfNewLogs()
            };

            this.statusBox = statusBox;
        }

        /// <summary>
        /// Initializes header
        /// </summary>
        private void InitializeUIHeader()
        {
            this.uiHeader = new UIHeader()
            {
                Type = UIHeaderType.none,
                Title = string.Empty
            };
        }

        /// <summary>
        /// Initializes current company
        /// Call ONLY if user is already initialized and IS authenticated
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="userName">UserName</param>
        private void InitializeCurrentCompany(string userName, string userId)
        {
            this.currentCompany = GetCompanyOfUser(userName, userId);
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
                            // ----- Process current user ------- //
                            var cacheKey = "BuilderAbstract.GetCurrentUser";
                            int cacheMinutes = 60;
                            var cacheSetup = this.Services.CacheService.GetSetup<ICurrentUser>(cacheKey, cacheMinutes);
                            cacheSetup.Dependencies = new List<string>()
                            {
                                EntityKeys.KeyUpdate<ApplicationUser>(currentIdentity.Name),
                                EntityKeys.KeyUpdateAny<ApplicationUser>()
                            };
                            cacheSetup.ObjectStringID = currentIdentity.Name;

                            var user = this.Services.CacheService.GetOrSet<ICurrentUser>(() => GetApplicationUser(currentIdentity.Name, currentIdentity.AuthenticationType), cacheSetup);

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
            var user = this.services.IdentityService.GetAll()
                .Where(m => m.UserName == userName)
                .Select(m => new CurrentUser()
                {
                    UserName = m.UserName,
                    Id = m.Id,
                    AuthenticationType = authenticationType,
                    IsAuthenticated = true,
                    FirstName = m.FirstName,
                    LastName = m.LastName
                })
                .FirstOrDefault();
            
            if (user == null)
            {
                return null;
            }

            // get roles of user
            user.Roles = userManager.GetRoles(user.Id);

            // set privilege level
            user.Privilege = PrivilegeHelper.GetPrivilegeLevel(user.Roles, PrivilegeLevel.Authenticated);

            return user;
        }


        /// <summary>
        /// Gets company of current user or null if it doesnt exist
        /// </summary>
        /// <param name="applicationUserId">applicationUserId</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <returns>Company of current user or null if not found</returns>
        private ICurrentCompany GetCompanyOfUserInternal(string applicationUserId)
        {
            var company = this.services.CompanyService.GetAll()
                .Where(m => m.ApplicationUserId == applicationUserId)
                .Select(m => new CurrentCompany()
                {
                    CompanyID = m.ID,
                    CompanyName = m.CompanyName,
                    CompanyCreatedByApplicationUserId = m.ApplicationUser.Id,
                    CompanyCreatedByApplicationUserName = m.ApplicationUser.UserName,
                    CompanyGUID = m.CompanyGUID
                })
                .FirstOrDefault();

            return company;
        }

        /// <summary>
        /// Gets company of current user from cache
        /// Empty company is used if user hasnt created any company yet
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns>Company of current user (emnpty object is returned if company does not exist)</returns>
        private ICurrentCompany GetCompanyOfUser(string userName, string applicationUserId)
        {
            var cacheKey = "BuilderAbstract.GetCompanyOfUserFromCache." + userName; // key under which company of user will be stored
            int cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<ICurrentCompany>(cacheKey, cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
                            {
                                EntityKeys.KeyDeleteAny<Entity.Company>(),
                                EntityKeys.KeyCreateAny<Entity.Company>(),
                                EntityKeys.KeyUpdateAny<Entity.Company>(),
                            };

            var company = this.Services.CacheService.GetOrSet<ICurrentCompany>(() => GetCompanyOfUserInternal(applicationUserId), cacheSetup);

            if (company != null)
            {
                return company;
            }

            return new CurrentCompany();
        }

        /// <summary>
        /// Gets status box from cache
        /// </summary>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns>Status box</returns>
        private int GetNumberOfNewMessages(string applicationUserId)
        {
            var cacheKey = "BuilderAbstract.GetStatusBox";
            int cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<IStatusBox>(cacheKey, cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
                            {
                                EntityKeys.KeyUpdateAny<Entity.Message>(),
                                EntityKeys.KeyDeleteAny<Entity.Message>(),
                                EntityKeys.KeyCreateAny<Entity.Message>(),
                            };
            cacheSetup.ObjectStringID = applicationUserId;

            var newMessages = this.Services.CacheService.GetOrSet<IntWrapper>(() => GetNumberOfNewMessagesInternal(applicationUserId), cacheSetup);

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
            var newMessagesQuery = this.Services.MessageService.GetAll()
                .Where(m => m.RecipientApplicationUserId == applicationUserId && !m.IsRead)
                .Select(m => new
                {
                    ID = m.ID
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
            var latestReadLogIDCookieName = AppConfig.CookieNames.LatestReadLogID;

            var latestReadLogID = this.Services.CookieService.GetCookieValue(latestReadLogIDCookieName);
            if (string.IsNullOrEmpty(latestReadLogID))
            {
                // cookie was not yet initialized, get all events
                return (GetNumberOfEventsByThreshold(0));
            }

            return (GetNumberOfEventsByThreshold(Convert.ToInt32(latestReadLogID)));
        }

        private int GetNumberOfEventsByThreshold(int logIDThreshold)
        {
            int cacheMinutes = 1; // cache only for 1 minute
            var cacheSetup = this.Services.CacheService.GetSetup<IntWrapper>(this.GetSource(), cacheMinutes);
            cacheSetup.ObjectID = logIDThreshold;

            var eventsQuery = this.Services.LogService.GetAll()
               .Where(m => m.ID > logIDThreshold)
               .Select(m => new IntWrapper()
               {
                   Value = m.ID
               })
               .OrderByDescending(m => m);

            var events = this.Services.CacheService.GetOrSet(() => eventsQuery.ToList(), cacheSetup);

            return events.Count;
        }

        #endregion
    }
}
