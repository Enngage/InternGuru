using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Forms;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    public class CompanyController : BaseController
    {

        #region Builder 

        readonly CompanyBuilder _companyBuilder;

        #endregion

        #region Controller

        public CompanyController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _companyBuilder = companyBuilder;
        }

        #endregion

        #region Actions

        [Route("Firma/{codeName}", Name = "CompanyIndex")]
        public async Task<ActionResult> Index(string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                return HttpNotFound();
            }

            var model = await _companyBuilder.BuildDetailViewAsync(codeName);

            if (model == null)
            {
                return HttpNotFound();
            }

            // log company profile view activity
            await _companyBuilder.LogCompanyProfileViewActivityAsync(model.Company.ID);

            return View("~/Views/Company/Index.cshtml", model);
        }

        #endregion

        #region POST actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Firma/{codeName}")]
        public async Task<ActionResult> Contact(CompanyContactUsForm form)
        {
            const string view = "~/Views/Company/Index.cshtml";

            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form);

                model.Anchor = "_ContactSection";

                return View(view, model);
            }

            try
            {
                await _companyBuilder.CreateMessage(form);

                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName);

                model.Anchor = "_ContactSection";

                // set form status
                model.ContactUsForm.FormResult.IsSuccess = true;

                return View(view, model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form);

                model.Anchor = "_ContactSection";

                return View(view, model);
            }
        }

        #endregion
    }
}