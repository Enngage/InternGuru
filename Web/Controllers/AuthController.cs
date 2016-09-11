using Core.Context;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Auth.Forms;
using UI.Builders.Auth.Views;
using UI.Builders.Company;
using UI.Builders.Master;

namespace Web.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {
        AuthBuilder authBuilder;

        public AuthController(IAppContext appContext, MasterBuilder masterBuilder, AuthBuilder authBuilder) : base (appContext, masterBuilder)
        {
            this.authBuilder = authBuilder;
        }

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
            return View();
        }

        public async Task<ActionResult> Avatar()
        {
            return View();
        }

        public async Task<ActionResult> Conversation()
        {
            return View();
        }

        #endregion

        #region POST methods

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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(await authBuilder.BuildEditCompanyViewAsync(form));
            }
        }

        #endregion
    }
}