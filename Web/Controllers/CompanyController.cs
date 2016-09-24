using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Forms;
using UI.Builders.Master;
using UI.Exceptions;

namespace Web.Controllers
{
    public class CompanyController : BaseController
    {
        CompanyBuilder companyBuilder;

        public CompanyController(IAppContext appContext, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, masterBuilder)
        {
            this.companyBuilder = companyBuilder;
        }

        #region Actions

        public async Task<ActionResult> Index(string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                return HttpNotFound();
            }

            var model = await companyBuilder.BuildDetailViewAsync(codeName);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion

        #region POST actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CompanyContactUsForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                return View(await companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form));
            }

            try
            {
                await companyBuilder.CreateMessage(form);

                var model = await companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, null);

                // set form status
                model.ContactUsForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                return View(await companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form));
            }
        }

        #endregion
    }
}