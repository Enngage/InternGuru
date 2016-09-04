using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Company;
using UI.Builders.Master;

namespace Web.Controllers
{
    public class CompaniesController : BaseController
    {
        CompanyBuilder companyBuilder;

        public CompaniesController(IAppContext appContext, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, masterBuilder)
        {
            this.companyBuilder = companyBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int? page)
        {
            var model = await companyBuilder.BuildIndexViewAsync(page);

            return View(model);
        }

        #endregion
    }
}