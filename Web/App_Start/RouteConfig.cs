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
                url: "Company/{codeName}",
                defaults: new { controller = "Company", action = "Index", codeName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BrowseInternships",
                url: "Internships/{category}",
                defaults: new { controller = "Internships", action = "Index", category = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
