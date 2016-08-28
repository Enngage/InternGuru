using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Common.Loc;
using Microsoft.Owin.Security.Facebook;
using Web.Lib.Ninject;
using Ninject;
using Common.Loc.Ninject;
using Common.Config;
using Ninject.Web.Common.OwinHost;
using Entity;
using Microsoft.Owin.Security.DataProtection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web
{
    public partial class Startup
    {
        /// <summary>
        /// Data protectien needs to be here because of DI
        /// Source: http://stackoverflow.com/questions/30474214/no-iusertokenprovider-is-registered-when-using-dependency-injection
        /// </summary>
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }
        public static IDataProtectionProvider GetDataProtectionProvider()
        {
            return DataProtectionProvider;
        }

        /// <summary>
        /// Initializes application. Keep the method call order
        /// 
        /// Authentication config -> http://go.microsoft.com/fwlink/?LinkId=301864
        /// </summary>
        /// <param name="app"></param>
        public void InitializeAuth(IAppBuilder app)
        {
            // Register global kernel
            KernelProvider.SetKernel(GetKernel());

            // Use ninject as middleware 
            app.UseNinjectMiddleware(GetKernel);

            // Configure authentication
            ConfigureAuth(app);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            // assign data protection for DI
            DataProtectionProvider = app.GetDataProtectionProvider();

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            var fbOptions = new FacebookAuthenticationOptions();
            fbOptions.AppId = AppConfig.FacebookAppID;
            fbOptions.AppSecret = AppConfig.FacebookAppSecret;
            fbOptions.Scope.Add("email");
            app.UseFacebookAuthentication(fbOptions);


            var gOptions = new GoogleOAuth2AuthenticationOptions();
            gOptions.ClientId = AppConfig.GoogleClientID;
            gOptions.ClientSecret = AppConfig.GoogleClientSecret;
            gOptions.Scope.Add("email");
            app.UseGoogleAuthentication(gOptions);
        }

        private IKernel GetKernel()
        {
            return NinjectHelper.GetKernel(NinjectKernelType.Web);
        }
    }
}