using Core.Config;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using UI.Attributes;
using UI.Builders.FineUpload.Binders;
using UI.Builders.FineUpload.Models;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // model binders
            ModelBinders.Binders.Add(typeof(FineUploadModel), new FIneUploadBinder());

            // Model attributes
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ValidInteger), typeof(ValidIntegerValidator));

            SystemConfig.SetServerRootPath(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath);
            SystemConfig.SetServerVirtualRootPath(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            SystemConfig.DomainName = Dns.GetHostName();
        }
    }
}
