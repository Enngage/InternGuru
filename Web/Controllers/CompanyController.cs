using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Enum;
using UI.Builders.Company.Forms;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    public class CompanyController : BaseController
    {
        readonly CompanyBuilder _companyBuilder;

        public CompanyController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _companyBuilder = companyBuilder;
        }

        #region Actions

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

            // set tab
            model.ActiveTab = CompanyDetailMenuEnum.About;

            // log company profile view activity
            await _companyBuilder.LogCompanyProfileViewActivityAsync(model.Company.ID);

            return View("~/Views/Company/Tabs/About.cshtml", model);
        }

        public async Task<ActionResult> Internships(string codeName)
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

            // set tab
            model.ActiveTab = CompanyDetailMenuEnum.Internships;

            // log company profile view activity
            await _companyBuilder.LogCompanyProfileViewActivityAsync(model.Company.ID);

            return View("~/Views/Company/Tabs/Internships.cshtml", model);
        }

        public async Task<ActionResult> Theses(string codeName)
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

            // set tab
            model.ActiveTab = CompanyDetailMenuEnum.Theses;

            // log company profile view activity
            await _companyBuilder.LogCompanyProfileViewActivityAsync(model.Company.ID);

            return View("~/Views/Company/Tabs/Theses.cshtml", model);
        }

        public async Task<ActionResult> Contact(string codeName)
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

            // set tab
            model.ActiveTab = CompanyDetailMenuEnum.Contact;

            // log company profile view activity
            await _companyBuilder.LogCompanyProfileViewActivityAsync(model.Company.ID);

            return View("~/Views/Company/Tabs/Contact.cshtml", model);
        }

        #endregion

        #region POST actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CompanyContactUsForm form)
        {
            const string contactView = "~/Views/Company/Tabs/Contact.cshtml";
            const CompanyDetailMenuEnum activeTab = CompanyDetailMenuEnum.Contact;

            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form);
                model.ActiveTab = activeTab;
                return View(contactView, model);
            }

            try
            {
                await _companyBuilder.CreateMessage(form);

                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName);

                // set active tab
                model.ActiveTab = activeTab;

                // set form status
                model.ContactUsForm.FormResult.IsSuccess = true;

                return View(contactView, model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form);
                model.ActiveTab = activeTab;

                return View(contactView, model);
            }
        }

        #endregion
    }
}