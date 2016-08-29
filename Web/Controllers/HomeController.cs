using Common.Loc;
using System.Web.Mvc;
using Ninject;
using Core.Services;
using System;
using System.Threading.Tasks;
using UI.Abstract;
using Core.Context;
using UI.Builders.Master;
using UI.Builders.Master.Views;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IAppContext appContext, MasterBuilder masterBuilder) : base (appContext, masterBuilder) { }

        public ActionResult Index()
        {
            var model = new MasterView();
            return View(model);
        }

        public async Task<ActionResult> About()
        {
            ViewBag.Message = "Your application description page.";

            var logService = KernelProvider.Kernel.Get<ILogService>();
            await logService.LogExceptionAsync(new Exception("Something bad happened"));

            await logService.DeleteAsync(1);

            var logToEdit = await logService.GetAsync(2);
            if (logToEdit != null)
            {
                logToEdit.InnerException = "ahoj";

                await logService.UpdateAsync(logToEdit);
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}