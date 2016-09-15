using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
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
    }
}