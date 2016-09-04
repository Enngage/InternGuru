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

        public async Task<ActionResult> RegisterCompany()
        {
            return View();
        }

        public async Task<ActionResult> EditCompany()
        {
            return View();
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
    }
}