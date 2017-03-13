using Service.Context;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers
{
    public class ErrorController : BaseController
    {

        public ErrorController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
        }

        #region Actions

        [Route("404")]
        public ActionResult NotFound()
        {
            return View("~/Views/shared/404.cshtml");
        }

        #endregion
    }
}