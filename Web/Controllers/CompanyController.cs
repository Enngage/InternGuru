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

        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await companyBuilder.BuildDetailViewAsync(id ?? 0);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion
    }
}