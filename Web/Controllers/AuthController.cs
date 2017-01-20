﻿using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Auth;
using UI.Builders.Auth.Forms;
using UI.Builders.Master;
using UI.Builders.Shared.Models;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {

        #region Builder

        readonly AuthBuilder _authBuilder;

        #endregion

        #region Constructor

        public AuthController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, AuthBuilder authBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _authBuilder = authBuilder;
        }

        #endregion

        #region Actions

        public async Task<ActionResult> Index(int? page)
        {
            var model = await _authBuilder.BuildIndexViewAsync(page);

            return View(model);
        }

        public async Task<ActionResult> Theses(int? page)
        {
            var model = await _authBuilder.BuildThesesVieAsync(page);

            return View(model);
        }

        public async Task<ActionResult> Internships(int? page)
        {
            var model = await _authBuilder.BuildInternshipsViewAsync(page);

            return View(model);
        }

        public async Task<ActionResult> RegisterCompany()
        {
            var model = await _authBuilder.BuildRegisterCompanyViewAsync();

            return View(model);
        }

        public async Task<ActionResult> EditCompany()
        {
            var model = await _authBuilder.BuildEditCompanyViewAsync();

            if (model == null)
            {
                // company does not exist
                return HttpNotFound();
            }

            return View(model);
        }

        public async Task<ActionResult> NewInternship()
        {
            var model = await _authBuilder.BuildNewInternshipViewAsync();

            return View(model);
        }

        public async Task<ActionResult> EditInternship(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _authBuilder.BuildEditInternshipViewAsync((int) id);

            // internship was not found
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        public async Task<ActionResult> NewThesis()
        {
            var model = await _authBuilder.BuildNewThesisViewAsync();

            return View(model);
        }

        public async Task<ActionResult> EditThesis(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _authBuilder.BuildEditThesisViewAsync((int) id);

            // thesis was not found
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        public async Task<ActionResult> EditProfile()
        {
            var model = await _authBuilder.BuildEditProfileViewAsync();

            return View(model);
        }

        public async Task<ActionResult> Avatar()
        {
            var model = await _authBuilder.BuildAvatarViewAsync();

            return View(model);
        }

        public async Task<ActionResult> CompanyGallery()
        {
            var model = await _authBuilder.BuildCompanyGalleryViewAsync();

            return View(model);
        }

        public async Task<ActionResult> Conversation(string id, int? page)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var model = await _authBuilder.BuildConversationViewAsync(id, page);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion

        #region POST methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Conversation(AuthMessageForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form));
            }

            try
            {
                await _authBuilder.CreateMessage(form);

                var model = await _authBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form);

                // set form status
                model.MessageForm.FormResult.IsSuccess = true;

                // clear the form
                model.MessageForm.Message = string.Empty;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildConversationViewAsync(form.RecipientApplicationUserId, null, form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Avatar(AuthAvatarUploadForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildAvatarViewAsync());
            }

            try
            {
                _authBuilder.UploadAvatar(form);

                var model = await _authBuilder.BuildAvatarViewAsync();

                model.AvatarForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildAvatarViewAsync());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompanyGallery(AuthCompanyGalleryUploadForm form)
        {
            var model = await _authBuilder.BuildCompanyGalleryViewAsync();

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                // upload files
                _authBuilder.UploadCompanyGalleryFiles(Request);

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(AuthEditProfileForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditProfileViewAsync(form));
            }

            try
            {
                // edit profile
                await _authBuilder.EditProfile(form);

                var model = await _authBuilder.BuildEditProfileViewAsync(form);

                // set form status
                model.ProfileForm.FormResult.IsSuccess = true;

                // set flag indicating whether user's e-mail is still visible to others or not
                model.AuthMaster.EmailVisibleForPeople = string.IsNullOrEmpty(model.ProfileForm.FirstName) && string.IsNullOrEmpty(model.ProfileForm.Nickname);

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditProfileViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildNewInternshipViewAsync(form));
            }

            try
            {
                // create internship
                var internshipID = await _authBuilder.CreateInternship(form);

                var model = await _authBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                // update InternshipID
                model.InternshipForm.ID = internshipID;

                // edit view name
                var editView = "~/Views/Auth/EditInternship.cshtml";

                return View(editView, model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildNewInternshipViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditInternshipViewAsync(form));
            }

            try
            {
                // edit internship
                await _authBuilder.EditInternship(form);

                var model = await _authBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditInternshipViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildRegisterCompanyViewAsync(form));
            }

            try
            {
                // create company
                var companyID = await _authBuilder.CreateCompany(form);

                var model = await _authBuilder.BuildRegisterCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                // update CompanyID
                model.CompanyForm.ID = companyID;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildRegisterCompanyViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditCompanyViewAsync(form));
            }

            try
            {
                // edit company
                await _authBuilder.EditCompany(form);

                var model = await _authBuilder.BuildEditCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditCompanyViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewThesis(AuthAddEditThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildNewThesisViewAsync(form));
            }

            try
            {
                // create thesis
                var thesisID = await _authBuilder.CreateThesis(form);

                var model = await _authBuilder.BuildNewThesisViewAsync(form);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                // update thesis ID
                model.ThesisForm.ID = thesisID;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildNewThesisViewAsync(form));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditThesis(AuthAddEditThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(await _authBuilder.BuildEditThesisViewAsync(form));
            }

            try
            {
                // edit thesis
                await _authBuilder.EditThesis(form);

                var model = await _authBuilder.BuildEditThesisViewAsync(form);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(await _authBuilder.BuildEditThesisViewAsync(form));
            }
        }

        #endregion
    }
}