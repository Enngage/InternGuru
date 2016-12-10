using Core.Helpers.Privilege;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Service.Context;
using System.Web.Mvc;
using System.Web.Security;
using UI.Base;
using UI.Builders.Master;
using UI.Events;
using Web.Lib.Authorize;

namespace Web.Controllers.System
{
   
    public class SystemController : BaseController
    {
        #region Constructor

        public SystemController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }
    }
}