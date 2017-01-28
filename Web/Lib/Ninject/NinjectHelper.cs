using Cache;
using Service.Context;
using EmailProvider;
using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Web;
using System.Web.Mvc;
using Core.Loc;
using UI.Builders.Services;
using UI.Events;
using UI.UIServices;
using UI.Builders.Shared.Models;
using Identity;
using Service.Services;
using Service.Services.Activities;
using Service.Services.Companies;
using Service.Services.Countries;
using Service.Services.Currencies;
using Service.Services.Emails;
using Service.Services.Identity;
using Service.Services.Internships;
using Service.Services.Languages;
using Service.Services.Logs;
using Service.Services.Messages;
using Service.Services.Thesis;
using UI.RazorExtensions;

namespace Web.Lib.Ninject
{
    /// <summary>
    /// Helpers class that is used to initialize Kernel and map classes
    /// </summary>
    public static class NinjectHelper
    {
        public static IKernel GetKernel(NinjectKernelType kernelType)
        {
            var kernel = new StandardKernel();

            kernel = InitializeCommonServices(kernel);

            try
            {
                if (kernelType == NinjectKernelType.Hangfire)
                {
                    return InitializeHangfireKernel(kernel);
                }
                if (kernelType == NinjectKernelType.Web)
                {
                    return InitializeWebKernel(kernel);
                }
                throw new Exception("Invalid Kernel type");
            }
            catch (Exception)
            {
                kernel.Dispose();
                throw;
            }
        }

        private static StandardKernel InitializeCommonServices(StandardKernel kernel)
        {
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            return kernel;
        }

        #region Web kernel

        private static IKernel InitializeWebKernel(IKernel kernel)
        {
            // AppContext
            var contextConfig = new AppContextConfig()
            {
                AutoDetectChanges = true
            };

            // bind app context with additional parameters
            kernel.Bind<IAppContext>().ToMethod(context => new Service.Context.AppContext(contextConfig)).InRequestScope();

            // Authentication 
            // taken from http://stackoverflow.com/questions/36239743/how-to-inject-usermanager-signinmanager
            kernel.Bind<IUserStore<ApplicationUser>>().ToMethod(m => new UserStore<ApplicationUser>(new Service.Context.AppContext(contextConfig)));
            kernel.Bind<ApplicationUserManager>().ToSelf().InRequestScope();
            kernel.Bind<ApplicationSignInManager>().ToSelf().InRequestScope();
            kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication).InRequestScope();

            // IDataProtection for e-mail confirmation
            kernel.Bind<IDataProtectionProvider>().ToMethod(m => Startup.GetDataProtectionProvider()).InRequestScope();

            // Services loader
            kernel.Bind<IServicesLoader>().To<ServicesLoader>().InRequestScope();

            // Cache service needs additional parameters and can be used as singleton
            kernel.Bind<ICacheService>().ToMethod(m => new CacheService(Core.Config.AppConfig.DisableCaching, Core.Config.AppConfig.DefaultCacheMinutes)).InSingletonScope();

            // Helpers Helpers - request scope
            kernel.Bind<IUIHelpers>().To<UIHelpers>().InRequestScope();

            // Services - they need to be in RequestScope, otherwise they may throw infinite recursion exceptions when a service is used inside another service
            kernel.Bind<ILogService>().To<LogService>().InRequestScope();
            kernel.Bind<IEmailProvider>().To<GoogleEmailProvider>().InRequestScope();
            kernel.Bind<IEmailMessage>().To<EmailMessage>().InRequestScope();
            kernel.Bind<ICompanyService>().To<CompanyService>().InRequestScope();
            kernel.Bind<ICompanyCategoryService>().To<CompanyCategoryService>().InRequestScope();
            kernel.Bind<IInternshipService>().To<InternshipService>().InRequestScope();
            kernel.Bind<IInternshipCategoryService>().To<InternshipCategoryService>().InRequestScope();
            kernel.Bind<IIdentityService>().To<Identityservice>().InRequestScope();
            kernel.Bind<IFileProvider>().To<FileProvider>().InRequestScope();
            kernel.Bind<IMessageService>().To<MessageService>().InRequestScope();
            kernel.Bind<IInternshipDurationTypeService>().To<InternshipDurationTypeService>().InRequestScope();
            kernel.Bind<InternshipAmountTypeService>().To<InternshipAmountTypeService>().InRequestScope();
            kernel.Bind<ICountryService>().To<CountryService>().InRequestScope();
            kernel.Bind<ICurrencyService>().To<CurrencyService>().InRequestScope();
            kernel.Bind<ICompanySizeService>().To<CompanySizeService>().InRequestScope();
            kernel.Bind<IInternshipAmountTypeService>().To<InternshipAmountTypeService>().InRequestScope();
            kernel.Bind<IThesisTypeService>().To<ThesisTypeService>().InRequestScope();
            kernel.Bind<IThesisService>().To<ThesisService>().InRequestScope();
            kernel.Bind<IEmailTemplateService>().To<EmailTemplateService>().InRequestScope();
            kernel.Bind<IServiceEvents>().To<ServiceEvents>().InRequestScope();
            kernel.Bind<ISystemContext>().To<SystemContext>().InRequestScope();
            kernel.Bind<IIdentityMessageService>().To<IdentityMessageService>().InRequestScope();
            kernel.Bind<ICookieService>().To<CookieService>().InRequestScope();
            kernel.Bind<ILanguageService>().To<LanguageService>().InRequestScope();
            kernel.Bind<IHomeOfficeOptionService>().To<HomeOfficeOptionService>().InRequestScope();
            kernel.Bind<IStudentStatusOptionService>().To<StudentStatusOptionService>().InRequestScope();
            kernel.Bind<IActivityService>().To<ActivityService>().InRequestScope();
            kernel.Bind<IEmailService>().To<EmailService>().InRequestScope();
            kernel.Bind<IEventsLoader>().To<EventsLoader>().InRequestScope();
            kernel.Bind<IServiceDependencies>().To<ServiceDependencies>().InRequestScope();

            return kernel;
        }

        #endregion

        #region Kernel for Hangfire

        private static IKernel InitializeHangfireKernel(IKernel kernel)
        {
            // AppContext
            var contextConfig = new AppContextConfig()
            {
                AutoDetectChanges = true
            };
            kernel.Bind<IAppContext>().ToMethod(context => new Service.Context.AppContext(contextConfig)).InThreadScope();

            // Authentication 
            // taken from http://stackoverflow.com/questions/36239743/how-to-inject-usermanager-signinmanager
            kernel.Bind<IUserStore<ApplicationUser>>().ToMethod(m => new UserStore<ApplicationUser>(new Service.Context.AppContext(contextConfig)));
            kernel.Bind<ApplicationUserManager>().ToSelf();
            kernel.Bind<ApplicationSignInManager>().ToSelf();
            kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication);

            // Services - in Thread scope because of the hangfire
            return kernel;
        }

        #endregion
    }
}