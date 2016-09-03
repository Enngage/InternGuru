using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Company;
using UI.Builders.Master;

namespace Web.Controllers
{
    public class AuthController : BaseController
    {

        public AuthController(IAppContext appContext, MasterBuilder masterBuilder) : base (appContext, masterBuilder)
        {
        }

        #region Actions

        public async Task<ActionResult> Index(int? page)
        {
            return View();
        }

        public async Task<ActionResult> RegisterCompany(int? page)
        {
            return View();
        }

        public async Task<ActionResult> NewInternship(int? page)
        {
            return View();
        }

        #endregion
    }
}