﻿using System;
using System.Runtime.CompilerServices;
using System.Linq;

using Core.Context;
using System.Web;
using Cache;
using System.Collections.Generic;
using Entity;
using UI.Builders.Shared;
using UI.Builders.Services;

namespace UI.Base
{
    public abstract class BaseBuilder : IDisposable
    {

        #region Variables

        private IAppContext appContext;
        private ICurrentUser currentUser;
        private ICurrentCompany currentCompany;

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
        /// Cache service
        /// </summary>
        protected ICacheService CacheService
        {
            get
            {
                return this.services.CacheService;
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
        /// <param name="appContext">appContext</param>
        /// <param name="IServicesLoader">services loader used to initialize all services</param>
        public BaseBuilder(
            IAppContext appContext,
            IServicesLoader servicesLoader
            )
        {
            this.appContext = appContext;
            this.services = servicesLoader;

            // Initialize current user
            InitializeCurrentUser();

            // Initialize company of current user if user is authenticated
            if (CurrentUser.IsAuthenticated)
            {
                InitializeCurrentCompany();
            }
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
        /// Initializes current company
        /// Call ONLY if user is already initialized and IS authenticated
        /// </summary>
        private void InitializeCurrentCompany()
        {
            this.currentCompany = GetCompanyOfUser(this.CurrentUser.UserName, this.CurrentUser.Id);
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
                            var cacheKey = "BuilderAbstract.GetCurrentUser." + currentIdentity.Name; // key under which given user will be stored
                            int cacheMinutes = 60;
                            var cacheSetup = this.CacheService.GetSetup<ICurrentUser>(cacheKey, cacheMinutes);
                            cacheSetup.Dependencies = new List<string>()
                            {
                                ApplicationUser.KeyUpdate<ApplicationUser>(currentIdentity.Name),
                                ApplicationUser.KeyUpdateAny<ApplicationUser>()
                            };

                            var user = this.CacheService.GetOrSet<ICurrentUser>(() => GetApplicationUser(currentIdentity.Name, currentIdentity.AuthenticationType), cacheSetup);

                            if (user == null)
                            {
                                // user was not found in DB
                                return new CurrentUser()
                                {
                                    IsAuthenticated = false
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
                IsAuthenticated = false
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
                    CompanyCreatedByApplicationUserName = m.ApplicationUser.UserName
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
            var cacheSetup = this.CacheService.GetSetup<ICurrentCompany>(cacheKey, cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
                            {
                                Entity.Company.KeyDeleteAny<Entity.Company>(),
                                Entity.Company.KeyCreateAny<Entity.Company>(),
                                Entity.Company.KeyUpdateAny<Entity.Company>(),
                            };

            var company = this.CacheService.GetOrSet<ICurrentCompany>(() => GetCompanyOfUserInternal(applicationUserId), cacheSetup);

            if (company != null)
            {
                return company;
            }

            return new CurrentCompany();
        }

        #endregion
    }
}
