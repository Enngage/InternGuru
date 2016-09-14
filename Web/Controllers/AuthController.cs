﻿using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Auth.Forms;
using UI.Builders.Company;
using UI.Builders.Master;
using UI.Exceptions;

namespace Web.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {

        #region Builder

        AuthBuilder authBuilder;

        #endregion

        #region Constructor

        public AuthController(IAppContext appContext, MasterBuilder masterBuilder, AuthBuilder authBuilder) : base(appContext, masterBuilder)
        {
            this.authBuilder = authBuilder;
        }

        #endregion

        #region Actions

        public async Task<ActionResult> Index(int? page)
        {
            var model = await authBuilder.BuildIndexViewAsync();

            return View(model);
        }

        public async Task<ActionResult> RegisterCompany()
        {
            var model = await authBuilder.BuildRegisterCompanyViewAsync();

            return View(model);
        }

        public async Task<ActionResult> EditCompany()
        {
            var model = await authBuilder.BuildEditCompanyViewAsync();

            if (model == null)
            {
                // company does not exist
                return HttpNotFound();
            }

            return View(model);
        }

        public async Task<ActionResult> NewInternship()
        {
            var model = await authBuilder.BuildNewInternshipViewAsync();

            return View(model);
        }

        public async Task<ActionResult> EditInternship(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await authBuilder.BuildEditInternshipViewAsync(id ?? 0);

            // internship was not found
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        public async Task<ActionResult> EditProfile()
        {
            var model = await authBuilder.BuildEditProfileViewAsync();

            return View(model);
        }

        public ActionResult Avatar()
        {
            var model = authBuilder.BuildAvatarView();

            return View(model);
        }

        public async Task<ActionResult> Conversation()
        {
            return View();
        }

        #endregion

        #region POST methods

        [HttpPost]
        public ActionResult Avatar(AuthAvatarUploadForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(authBuilder.BuildAvatarView());
            }

            try
            {
                this.authBuilder.UploadAvatar(form);

                return View(authBuilder.BuildAvatarView());
            }
            catch(UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(authBuilder.BuildAvatarView());
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditProfile(AuthEditProfileForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(authBuilder.BuildEditProfileView(form));
            }

            try
            {
                // edit profile
                await authBuilder.EditProfile(form);

                var model = authBuilder.BuildEditProfileView(form);

                // set form status
                model.ProfileForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(authBuilder.BuildEditProfileView(form));
            }
        }

        [HttpPost]
        public async Task<ActionResult> NewInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(await authBuilder.BuildNewInternshipViewAsync(form));
            }

            try
            {
                // create internship
                var internshipID = await authBuilder.CreateInternship(form);

                var model = await authBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                // update InternshipID
                model.InternshipForm.ID = internshipID;

                // edit view name
                var editView = "~/Views/Auth/EditInternship.cshtml";

                return View(editView, model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(await authBuilder.BuildNewInternshipViewAsync(form));
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditInternship(AuthAddEditInternshipForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(await authBuilder.BuildEditInternshipViewAsync(form));
            }

            try
            {
                // edit internship
                await authBuilder.EditInternship(form);

                var model = await authBuilder.BuildEditInternshipViewAsync(form);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(await authBuilder.BuildEditInternshipViewAsync(form));
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegisterCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(await authBuilder.BuildRegisterCompanyViewAsync(form));
            }

            try
            {
                // create company
                var companyID = await authBuilder.CreateCompany(form);

                var model = await authBuilder.BuildRegisterCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                // update CompanyID
                model.CompanyForm.ID = companyID;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(await authBuilder.BuildRegisterCompanyViewAsync(form));
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditCompany(AuthAddEditCompanyForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(await authBuilder.BuildEditCompanyViewAsync(form));
            }

            try
            {
                // edit company
                await authBuilder.EditCompany(form);

                var model = await authBuilder.BuildEditCompanyViewAsync(form);

                // set form status
                model.CompanyForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(await authBuilder.BuildEditCompanyViewAsync(form));
            }
        }

        #endregion
    }
}