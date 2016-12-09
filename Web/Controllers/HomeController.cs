using Core.Loc;
using System.Web.Mvc;
using Ninject;
using Service.Services;
using System;
using System.Threading.Tasks;
using UI.Base;
using Service.Context;
using UI.Builders.Master;
using UI.Builders.Master.Views;
using Entity;
using System.Linq;
using System.Data.Entity;
using UI.Events;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder) : base (appContext, serviceEvents, masterBuilder) { }

        public async Task<ActionResult> Test()
        {
            var companyService = Core.Loc.KernelProvider.Kernel.Get<ICompanyService>();
            var masterBuilder = Core.Loc.KernelProvider.Kernel.Get<MasterBuilder>();
            var companyCategoryService = Core.Loc.KernelProvider.Kernel.Get<ICompanyCategoryService>();

            var category = await companyCategoryService.GetAll().FirstOrDefaultAsync();

            int companiesToCreate = 30;

            for(int i = 0; i < companiesToCreate; i++)
            {
                var company = new Company()
                {
                    ApplicationUserId = masterBuilder.CurrentUser.Id,
                    CompanyName = "Company_" + i,
                    Lat = 0,
                    LongDescription = "FE",
                    Lng = 0,
                    CompanyCategoryID = category.ID
                };

                await companyService.InsertAsync(company);
            }

            return View();
        }

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