using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Entity;
using UI.Base;
using Core.Context;
using UI.Builders.Master;
using UI.Builders.Account;
using UI.Builders.Account.Forms;
using UI.Events;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {

        #region Services

        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;
        private AccountBuilder accountBuilder;

        #endregion

        public AccountController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ApplicationUserManager userManager, ApplicationSignInManager signInManager, AccountBuilder accountBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accountBuilder = accountBuilder;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = accountBuilder.BuildLoginView();

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult EmailNotConfirmed()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginForm form, string returnUrl)
        {
            // get model
            var model = accountBuilder.BuildLoginView();

            // set submitted values
            model.LoginForm.UserName = form.UserName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Prevent users with not confirmed e-mail address from logging in
            var user = userManager.FindByName(form.UserName);

            if (user != null)
            {
                if (!userManager.IsEmailConfirmed(user.Id))
                {
                    return View("EmailNotConfirmed");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await signInManager.PasswordSignInAsync(form.UserName, form.Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelStateWrapper.AddError("Přihlášení se nezdařilo");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = this.accountBuilder.BuildRegisterView();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmailSent()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterForm form)
        {
            var model = accountBuilder.BuildRegisterView();
            model.RegisterForm.Email = form.Email;

            if (ModelState.IsValid)
            {
                // User name = e-mail
                var user = new ApplicationUser { UserName = form.Email, Email = form.Email };
                var result = await userManager.CreateAsync(user, form.Password);
                if (result.Succeeded)
                {
                    // do not log user - he needs to confirm e-mail first
                    // await signInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);

                    // send confirmation e-mail
                    await SendRegistrationEmail(user.Id);

                    // redirect to confirmation page
                    return RedirectToAction("ConfirmEmailSent");
                }

                bool unknownErrorAdded = false;
                bool emailTakenErrorAdded = false;

                foreach (var error in result.Errors)
                {
                    // translate errors
                    if (error.EndsWith("already taken.", System.StringComparison.OrdinalIgnoreCase))
                    {
                        if (!emailTakenErrorAdded)
                        {
                            this.ModelStateWrapper.AddError(string.Format("E-mail {0} je již zaregistrován", form.Email));
                            emailTakenErrorAdded = true;
                        }
                    }
                    else
                    {
                        // other errors
                        if (!unknownErrorAdded)
                        {
                            this.ModelStateWrapper.AddError("Nastala neočekavá chyba, opakujte prosím akci");
                            unknownErrorAdded = true;
                        }
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordForm form)
        {
            var model = accountBuilder.BuildForgotPasswordView();

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(form.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var email = accountBuilder.GetForgottenPasswordEmail(callbackUrl);

                await userManager.SendEmailAsync(user.Id, email.Subject, email.Body);

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordForm form)
        {
            var model = accountBuilder.BuildResetPasswordView();
            model.ResetPasswordForm.Email = form.Email;
            model.ResetPasswordForm.Code = form.Code;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // get user by email/username
            var user = await userManager.FindByNameAsync(model.ResetPasswordForm.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                this.ModelStateWrapper.AddError("Zadali jste neexistujícího uživatele");
                return View();
            }
            var result = await userManager.ResetPasswordAsync(user.Id, form.Code, form.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var model = accountBuilder.BuildExternalLoginConfirmationView();

            ViewBag.ReturnUrl = returnUrl;

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                var loginModel = accountBuilder.BuildLoginView();
                this.ModelStateWrapper.AddError("Autentizace nebyla úspěšná");
                return View("Login", loginModel);
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If we got here it means the user does not have local account
                    var email = loginInfo.Email;

                    // register user automatically if e-mail is available
                    if (!string.IsNullOrEmpty(email))
                    {
                        var user = new ApplicationUser { UserName = email, Email = email };
                        var resultUser = await userManager.CreateAsync(user);
                        if (resultUser.Succeeded)
                        {
                            resultUser = await userManager.AddLoginAsync(user.Id, loginInfo.Login);
                            if (resultUser.Succeeded)
                            {
                                await signInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);

                                // Send confirmation e-mail
                                await SendRegistrationEmail(user.Id);

                                return RedirectToLocal(returnUrl);
                            }
                        }
                        AddErrors(resultUser);

                        return View(model);
                    }
                    else
                    {
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        
                        model.ConfirmationForm.Email = loginInfo.Email;

                        return View("ExternalLoginConfirmation", model);
                    }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationForm form, string returnUrl)
        {
            var model = accountBuilder.BuildExternalLoginConfirmationView();
            model.ConfirmationForm.Email = form.Email;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = form.Email, Email = form.Email };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);

                        // send confirmation e-mail
                        await SendRegistrationEmail(user.Id);

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }

                if (signInManager != null)
                {
                    signInManager.Dispose();
                    signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        private async Task SendRegistrationEmail(string userId)
        {
            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);

            var registrationEmail = accountBuilder.GetRegistrationEmail(callbackUrl);

            await userManager.SendEmailAsync(userId, registrationEmail.Subject, registrationEmail.Body);
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}