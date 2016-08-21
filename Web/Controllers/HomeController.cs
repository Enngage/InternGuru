using Common.Loc;
using System.Web.Mvc;
using Ninject;
using Core.Services;
using System;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            ViewBag.Message = "Your application description page.";

            var logService = KernelProvider.Kernel.Get<ILogService>();
            await logService.LogExceptionAsync(new Exception("Something bad happened"));

            await logService.DeleteAsync(1);

            var logToEdit = await logService.Get(2);
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