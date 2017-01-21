using RouteLocalization.Mvc;
using Web.Controllers;

namespace Web.Lib.RouteLocalization
{
    /// <summary>
    /// Class used to register translations
    /// NOTE: Not needed until other cultures are added to the site
    /// </summary>
    public static class RouteTranslations
    {
        public static void AddRouteTranslations(this Localization localization)
        {
            // -------- EXAMPLES ------//

            //localization.ForCulture(RouteCultures.en)
            //    .ForController<HomeController>()
            //    .ForAction(x => x.Pricing())
            //    .AddTranslation("Pricing");


            //localization.ForCulture(RouteCultures.en)
            //    .ForController<HomeController>()
            //    .ForAction(x => x.Contact())
            //    .AddTranslation("Contact");

        }
    }
}