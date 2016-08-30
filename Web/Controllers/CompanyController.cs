using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Company;
using UI.Builders.Master;

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

        public async Task<ActionResult> Index(int? page)
        {
            var model = await companyBuilder.BuildIndexViewAsync(page);

            return View(model);
        }

        #endregion
    }
}