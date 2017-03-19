using System.Threading.Tasks;
using System.Web.Mvc;
using Service.Context;
using UI.Base;
using UI.Builders.Auth;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers.Auth
{
    [Authorize]
    public class AuthController : BaseController
    {

        #region Setup

        private const string GeneralActionPrefix = "uzivatel";

        protected const string CompanyActionPrefix = "ucet";
        protected const string CompanyViewsFolder = "~/Views/Auth/AuthCompany/";

        protected const string CandidateActionPrefix = "uchazec";
        protected const string CandidateViewsFolder = "~/Views/Auth/AuthCandidate/";

        #endregion

        #region Builder

        protected readonly AuthBuilder AuthBuilder;

        #endregion

        #region Constructor

        public AuthController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, AuthBuilder authBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            AuthBuilder = authBuilder;
        }

        #endregion

        #region Actions

        [Route(GeneralActionPrefix + "/AuthRedirect")]
        public async Task<ActionResult> AuthRedirect()
        {
            var model = await AuthBuilder.AuthMasterBuilder.BuildCompanyTypeIndexViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            // redirect user to user type selection if he hasn't set up his account
            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            // redirect to proper auth action otherwise
            return RedirectToAction(this.AuthBuilder.AuthCompanyBuilder.CurrentUser.AuthMasterAction, "Auth");
        }

        [Route(GeneralActionPrefix + "/Admin")]
        public async Task<ActionResult> Admin()
        {
            var model = await AuthBuilder.AuthMasterBuilder.BuildAdminViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route(GeneralActionPrefix + "/Nastaveni")]
        public async Task<ActionResult> UserSettings()
        {
            var model = await AuthBuilder.AuthMasterBuilder.BuildUserSettingsViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route(GeneralActionPrefix + "/TypUctu")]
        public async Task<ActionResult> UserTypeSelection()
        {
            var model = await AuthBuilder.AuthMasterBuilder.BuildCompanyTypeIndexViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }
           
            return ShowUserTypeSelection(model);
        }

        [Route(CompanyActionPrefix)]
        public async Task<ActionResult> CompanyTypeIndex()
        {
            var model = await AuthBuilder.AuthMasterBuilder.BuildCompanyTypeIndexViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CompanyViewsFolder}Index.cshtml", model);
        }

        [Route(CandidateActionPrefix)]
        public async Task<ActionResult> CandidateTypeIndex(int? page)
        {
            var model = await AuthBuilder.AuthMasterBuilder.BuildCandidateTypeIndexViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View($"{CandidateViewsFolder}Index.cshtml", model);
        }

        [Route(GeneralActionPrefix + "/UpravitProfil")]
        public async Task<ActionResult> EditProfile()
        {
            var model = await AuthBuilder.AuthProfileBuilder.BuildEditProfileViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route(GeneralActionPrefix + "/Avatar")]
        public async Task<ActionResult> Avatar()
        {
            var model = await AuthBuilder.AuthProfileBuilder.BuildAvatarViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route(GeneralActionPrefix + "/Konverzace/{id}")]
        public async Task<ActionResult> Conversation(string id, int? page)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var model = await AuthBuilder.AuthMessageBuilder.BuildConversationViewAsync(id, page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        [Route(GeneralActionPrefix + "/zpravy")]
        public async Task<ActionResult> Conversations(int? page)
        {
            var model = await AuthBuilder.AuthMessageBuilder.BuildConversationsViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.AuthMaster.ShowUserTypeSelectionView)
            {
                return ShowUserTypeSelection(model);
            }

            return View(model);
        }

        #endregion

        #region POST methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(GeneralActionPrefix + "/Konverzace/{id}")]
        public async Task<ActionResult> Conversation(AuthMessageForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await AuthBuilder.AuthMessageBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form));
            }

            try
            {
                await AuthBuilder.AuthMessageBuilder.CreateMessage(form);

                var model = await AuthBuilder.AuthMessageBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form);

                // set form status
                model.MessageForm.FormResult.IsSuccess = true;

                // clear the form
                model.MessageForm.Message = string.Empty;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await AuthBuilder.AuthMessageBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(GeneralActionPrefix + "/UpravitProfil")]
        public async Task<ActionResult> EditProfile(AuthEditProfileForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await AuthBuilder.AuthProfileBuilder.BuildEditProfileViewAsync(form));
            }

            try
            {
                // edit profile
                await AuthBuilder.AuthProfileBuilder.EditProfile(form);

                var model = await AuthBuilder.AuthProfileBuilder.BuildEditProfileViewAsync(form);

                // set form status
                model.ProfileForm.FormResult.IsSuccess = true;

                // set flag indicating whether user's e-mail is still visible to others or not
                model.AuthMaster.EmailVisibleForPeople = string.IsNullOrEmpty(model.ProfileForm.FirstName) && string.IsNullOrEmpty(model.ProfileForm.Nickname);

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await AuthBuilder.AuthProfileBuilder.BuildEditProfileViewAsync(form));
            }
        }

        #endregion

        #region User type selection GET methods

        [HttpGet]
        [Route(GeneralActionPrefix + "/Typ/Uchazec")]
        public async Task<ActionResult> SetCandidateType()
        {
            await AuthBuilder.AuthProfileBuilder.SetUserTypeAsync(false, true);

            return RedirectToAction("CandidateTypeIndex");
        }

        [HttpGet]
        [Route(GeneralActionPrefix + "/Typ/Firma")]
        public async Task<ActionResult> SetCompanyType()
        {
            await AuthBuilder.AuthProfileBuilder.SetUserTypeAsync(true, false);

            return RedirectToAction("CompanyTypeIndex");
        }

        #endregion

        #region Helper methods

        protected ActionResult ShowUserTypeSelection(AuthMasterView view)
        {
            return View("~/views/auth/UserTypeSelection.cshtml", view);
        }

        #endregion

    }
}