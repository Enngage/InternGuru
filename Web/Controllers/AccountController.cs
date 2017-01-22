using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Entity;
using UI.Base;
using Service.Context;
using UI.Builders.Master;
using UI.Builders.Account;
using UI.Builders.Account.Forms;
using UI.Events;
using System;
using Identity;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {

        #region Services

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly AccountBuilder _accountBuilder;

        #endregion

        public AccountController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ApplicationUserManager userManager, ApplicationSignInManager signInManager, AccountBuilder accountBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountBuilder = accountBuilder;
        }

        [AllowAnonymous]
        [Route("Prihlaseni")]
        public ActionResult Login(string returnUrl)
        {
            var model = _accountBuilder.BuildLoginView();

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [AllowAnonymous]
        [Route("Ucet/Neaktivni")]
        public ActionResult EmailNotConfirmed()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Prihlaseni")]
        public async Task<ActionResult> Login(LoginForm form, string returnUrl)
        {
            // get model
            var model = _accountBuilder.BuildLoginView();

            // set submitted values
            model.LoginForm.UserName = form.UserName;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Prevent users with not confirmed e-mail address from logging in
            var user = _userManager.FindByName(form.UserName);

            if (user != null)
            {
                if (!_userManager.IsEmailConfirmed(user.Id))
                {
                    return View("EmailNotConfirmed");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(form.UserName, form.Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    ModelStateWrapper.AddError("Přihlášení se nezdařilo");
                    return View(model);
            }
        }

        [AllowAnonymous]
        [Route("Registrace")]
        public ActionResult Register()
        {
            var model = _accountBuilder.BuildRegisterView();

            return View(model);
        }

        [AllowAnonymous]
        [Route("Ucet/RegistracniEmailOdeslan")]
        public ActionResult ConfirmEmailSent()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Registrace")]
        public async Task<ActionResult> Register(RegisterForm form)
        {
            var model = _accountBuilder.BuildRegisterView();
            model.RegisterForm.Email = form.Email;

            if (ModelState.IsValid)
            {
                // User name = e-mail
                var user = new ApplicationUser { UserName = form.Email, Email = form.Email };
                var result = await _userManager.CreateAsync(user, form.Password);
                if (result.Succeeded)
                {
                    // do not log user - he needs to confirm e-mail first
                    // await signInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);

                    // send confirmation e-mail
                    await SendRegistrationEmail(user.Id);

                    // redirect to confirmation page
                    return RedirectToAction("ConfirmEmailSent");
                }

                var unknownErrorAdded = false;
                var emailTakenErrorAdded = false;

                foreach (var error in result.Errors)
                {
                    // translate errors
                    if (error.EndsWith("already taken.", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!emailTakenErrorAdded)
                        {
                            ModelStateWrapper.AddError($"E-mail {form.Email} je již zaregistrován");
                            emailTakenErrorAdded = true;
                        }
                    }
                    else
                    {
                        // other errors
                        if (!unknownErrorAdded)
                        {
                            ModelStateWrapper.AddError("Nastala neočekavá chyba, opakujte prosím akci");
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
        [Route("Ucet/PotvrzeniEmailu")]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        [Route("Ucet/ZapomenuteHeslo")]
        public ActionResult ForgotPassword()
        {
            var model = _accountBuilder.BuildForgotPasswordView();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Ucet/ZapomenuteHeslo")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordForm form)
        {
            var model = _accountBuilder.BuildForgotPasswordView();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(form.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request?.Url?.Scheme);

                var email = _accountBuilder.GetForgottenPasswordEmail(callbackUrl);

                await _userManager.SendEmailAsync(user.Id, email.Subject, email.Body);

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        [Route("Ucet/ZapomenuteHesloPotvrzeni")]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        [Route("Ucet/ObnovaHesla")]
        public ActionResult ResetPassword(string code)
        {
            var model = _accountBuilder.BuildResetPasswordView();

            if (string.IsNullOrEmpty(code))
            {
                model.InvalidCodeToken = true;
                ModelStateWrapper.AddError("Nevalidní URL");
            }
            else
            {
                model.Code = code;
            }

            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Ucet/ObnovaHesla")]
        public async Task<ActionResult> ResetPassword(ResetPasswordForm form)
        {
            var model = _accountBuilder.BuildResetPasswordView();
            model.ResetPasswordForm.Email = form.Email;
            model.ResetPasswordForm.Code = form.Code;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // get user by email/username
            var user = await _userManager.FindByNameAsync(model.ResetPasswordForm.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelStateWrapper.AddError("Zadali jste neexistujícího uživatele");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, form.Code, form.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View(model);
        }

        [AllowAnonymous]
        [Route("Ucet/ObnovaHeslaPotvrzeni")]
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
            var model = _accountBuilder.BuildExternalLoginConfirmationView();

            ViewBag.ReturnUrl = returnUrl;

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                var loginModel = _accountBuilder.BuildLoginView();
                ModelStateWrapper.AddError("Autentizace nebyla úspěšná");
                return View("Login", loginModel);
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                default:
                    // If we got here it means the user does not have local account
                    var email = loginInfo.Email;

                    // register user automatically if e-mail is available
                    if (!string.IsNullOrEmpty(email))
                    {
                        var user = new ApplicationUser { UserName = email, Email = email };
                        var resultUser = await _userManager.CreateAsync(user);
                        if (resultUser.Succeeded)
                        {
                            resultUser = await _userManager.AddLoginAsync(user.Id, loginInfo.Login);
                            if (resultUser.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);

                                // Send confirmation e-mail
                                await SendRegistrationEmail(user.Id);

                                return RedirectToLocal(returnUrl);
                            }
                        }
                        AddErrors(resultUser);

                        // something wrong happened and user could not be signed in
                        var loginModel = _accountBuilder.BuildLoginView();
                        return View("Login", loginModel);
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
        [Route("Ucet/PortvrzeniExternihoUctu")]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationForm form, string returnUrl)
        {
            var model = _accountBuilder.BuildExternalLoginConfirmationView();
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
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);

                        // send confirmation e-mail
                        await SendRegistrationEmail(user.Id);

                        return RedirectToLocal(returnUrl);
                    }
                }
                 
                // check for name already taken errors (ending with "taken.") and translate them
                bool nameTakenErrorProcessed = false;
                foreach (var error in result.Errors)
                {
                    if (string.IsNullOrEmpty(error))
                    {
                        continue;
                    }

                    if (error.EndsWith("taken.", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!nameTakenErrorProcessed)
                        {
                            ModelStateWrapper.AddError($"E-mail {form.Email} je již zaregistrován.");
                            nameTakenErrorProcessed = true;
                        }
                    }
                }

                // add generic error if it wasnt the already registered name
                if (!nameTakenErrorProcessed)
                {
                    ModelStateWrapper.AddError("Nastala neočekáváná chyba, kterou jsme zaznamenali a brzy se na ni podíváme.");

                    // log error to investigate
                    this._accountBuilder.LogLoginError(string.Join(",", result.Errors));
                }
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
        [Route("Ucet/ExterniUcetNevalidni")]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

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

            return RedirectToAction("Index", "Auth");
        }


        private async Task SendRegistrationEmail(string userId)
        {
            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId, code }, protocol: Request?.Url?.Scheme);

            var registrationEmail = _accountBuilder.GetRegistrationEmail(callbackUrl);

            await _userManager.SendEmailAsync(userId, registrationEmail.Subject, registrationEmail.Body);
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

        public ActionResult SendCode(string returnurl)
        {
            throw new NotImplementedException();
        }
    }
}