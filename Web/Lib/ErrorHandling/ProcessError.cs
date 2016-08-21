using System;
using System.Web.Mvc;

using Ninject;
using Common.Loc;
using Core.Services;
using UI.Builders.Master.Views;

namespace Web.Lib.ErrorHandling
{
    // http://www.codeproject.com/Articles/850062/Exception-handling-in-ASP-NET-MVC-methods-explaine
    public class ProcessError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;

            // log error
            var logService = KernelProvider.Kernel.Get<ILogService>();
            logService.LogException(ex, filterContext.RequestContext.HttpContext.Request.RawUrl, filterContext?.Controller?.ControllerContext?.RequestContext?.HttpContext?.User?.Identity?.Name ?? null);

            filterContext.ExceptionHandled = true;
            string innerException = ex.InnerException != null ? ex.InnerException.ToString() : null;

            var errorView = new ErrorView(ex);

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary(errorView)
            };
        }
    }
}
