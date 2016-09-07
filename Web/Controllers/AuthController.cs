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

            return View(model);
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
                model.CompanyForm.FormResult.Success = true;

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

        #endregion
    }
}