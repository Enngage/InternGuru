using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers
{
    public class CompaniesController : BaseController
    {
        readonly CompanyBuilder _companyBuilder;

        public CompaniesController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _companyBuilder = companyBuilder;
        }

        #region Actions

        [Route("Firmy")]
        public async Task<ActionResult> Index(int? page)
        {
            var model = await _companyBuilder.BuildBrowseViewAsync(page);

            return View(model);
        }

        #endregion
    }
}