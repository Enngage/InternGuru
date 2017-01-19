using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Company",
                url: "Company/{codeName}/{action}",
                defaults: new { controller = "Company", action = "Index", codeName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Internship",
                url: "Internship/{id}/{codeName}",
                defaults: new { controller = "Internship", action = "Index", id = UrlParameter.Optional, codename = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Thesis",
              url: "Thesis/{id}/{codeName}",
              defaults: new { controller = "Thesis", action = "Index", id = UrlParameter.Optional, codename = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Internships",
                url: "Internships/{category}",
                defaults: new { controller = "Internships", action = "Index", category = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Theses",
                url: "Theses/{category}",
                defaults: new { controller = "Theses", action = "Index", category = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "FormRoute",
              url: "Form/{action}/{id}/{codeName}",
              defaults: new { controller = "Form", action = "Index", id = UrlParameter.Optional, codename = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
