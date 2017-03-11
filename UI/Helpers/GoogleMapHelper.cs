using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Base;
using UI.Modules.Breadcrumbs.Models;
using UI.Modules.Breadcrumbs.Views;
using UI.Modules.GoogleMaps.Views;

namespace UI.Helpers
{
    public class GoogleMapHelper : HelperBase
    {
        public GoogleMapHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Path to google map view
        /// </summary>
        private const string GoogleMapView = "~/views/modules/googlemaps/googlemap.cshtml";

        /// <summary>
        /// Gets Google map 
        /// </summary>
        /// <param name="lat">Lattitude</param>
        /// <param name="lng">Longtitude</param>
        /// <param name="title">title</param>
        /// <param name="zoom">Lattitude</param>
        /// <returns>Breadcrumbs</returns>
        public IHtmlString GetMap(double lat, double lng, string title, int zoom = 10)
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return this.WebViewPage.Html.Partial(GoogleMapView, new GoogleMapView()
            {
               Lat = lat,
               Lng = lng,
               MarkerTitle = title,
               Zoom = zoom
            });
        }
    }
}
