using System.Web.Mvc;
using UI.Base;
using Service.Context;
using UI.Builders.Master;
using UI.Builders.Master.Views;
using UI.Events;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder) : base (appContext, serviceEvents, masterBuilder) { }

        public ActionResult Index()
        {
            var model = new MasterView();
            return View(model);
        }

    }
}