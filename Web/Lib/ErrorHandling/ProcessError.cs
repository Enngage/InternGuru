using System.Web.Mvc;

using Ninject;
using Core.Loc;
using Service.Services.Logs;
using UI.Builders.Master.Views;
using UI.Builders.Master;

namespace Web.Lib.ErrorHandling
{
    /// <summary>
    /// Fires whenever an exception occurs and logs it 
    /// More info: http://www.codeproject.com/Articles/850062/Exception-handling-in-ASP-NET-MVC-methods-explaine
    /// </summary>
    public class ProcessError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            // get services
            var logService = KernelProvider.Kernel.Get<ILogService>();
            var masterBuilder = KernelProvider.Kernel.Get<MasterBuilder>();

            // mark exception as handled in the filter context
            filterContext.ExceptionHandled = true;

            // --- log exception ---- //
            var ex = filterContext.Exception;
            logService.LogException(ex, filterContext.RequestContext.HttpContext.Request.RawUrl, filterContext.Controller?.ControllerContext?.RequestContext?.HttpContext?.User?.Identity?.Name);

            // create error view
            var errorView = new ErrorView(ex);

            // initialize master model directly in HandleErrorAttribute because it is not called in controller
            errorView.Master = masterBuilder.GetMasterModel();

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary(errorView)
            };
        }
    }
}
