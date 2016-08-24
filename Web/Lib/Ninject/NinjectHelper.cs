using Cache;
using Common.Loc.Ninject;
using Core.Context;
using Core.Services;
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
                else if (kernelType == NinjectKernelType.Web)
                {
                    return InitializeWebKernel(kernel);
                }
                else
                {
                    throw new Exception("Invalid Kernel type");
                }
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

            kernel.Bind<Core.Context.IAppContext>().ToMethod(context => new Core.Context.AppContext(contextConfig)).InRequestScope();

            // Authentication 
            // taken from http://stackoverflow.com/questions/36239743/how-to-inject-usermanager-signinmanager
            kernel.Bind<IUserStore<ApplicationUser>>().ToMethod(m => new UserStore<ApplicationUser>(new Core.Context.AppContext(contextConfig)));
            kernel.Bind<ApplicationUserManager>().ToSelf();
            kernel.Bind<ApplicationSignInManager>().ToSelf();
            kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication);

            // IDataProtection for e-mail confirmation
            kernel.Bind<IDataProtectionProvider>().ToMethod(m => Startup.GetDataProtectionProvider()).InRequestScope();

            // Services - they need to be in RequestScope, otherwise they may throw infinite recursion exceptions in services
            kernel.Bind<ICacheService>().To<CacheService>().InRequestScope();
            kernel.Bind<ILogService>().To<LogService>().InRequestScope();
            kernel.Bind<IEmailProvider>().To<GoogleEmailProvider>().InRequestScope();
            kernel.Bind<IEmail>().To<Email>().InRequestScope();

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
            kernel.Bind<Core.Context.IAppContext>().ToMethod(context => new Core.Context.AppContext(contextConfig)).InThreadScope();

            // Authentication 
            // taken from http://stackoverflow.com/questions/36239743/how-to-inject-usermanager-signinmanager
            kernel.Bind<IUserStore<ApplicationUser>>().ToMethod(m => new UserStore<ApplicationUser>(new Core.Context.AppContext(contextConfig)));
            kernel.Bind<ApplicationUserManager>().ToSelf();
            kernel.Bind<ApplicationSignInManager>().ToSelf();
            kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication);

            // Services - in Thread scope because of the hangfire
            return kernel;
        }

        #endregion
    }
}