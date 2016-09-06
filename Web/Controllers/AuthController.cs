using Core.Context;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Auth.Forms;
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
            return View();
        }

        public async Task<ActionResult> NewInternship()
        {
            return View();
        }

        public async Task<ActionResult> EditInternship()
        {
            return View();
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
        public async Task<ActionResult> AddEditCompany(AuthAddEditCompanyForm form)
        {
            if (form.ID == 0)
            {
                // Create new company
                return await AddCompany(form);

            }
            else
            {
                // Update existing company
                return null;
            }
        }


        #endregion

        #region Helper methods

        public async Task<ActionResult> AddCompany(AuthAddEditCompanyForm form)
        {
            var viewPath = "~/Views/Auth/RegisterCompany.cshtml";

            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(viewPath, authBuilder.BuildRegisterCompanyView(form));
            }

            try
            {
                // create company
                var companyID = await authBuilder.CreateCompany(form);

                var model = authBuilder.BuildRegisterCompanyView(form);

                // set form status
                model.CompanyForm.FormResult.Success = true;

                // update CompanyID
                model.CompanyForm.ID = companyID;

                return View(viewPath, model);
            }
            catch (Exception ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(viewPath, authBuilder.BuildRegisterCompanyView(form));
            }
        }

        #endregion
    }
}