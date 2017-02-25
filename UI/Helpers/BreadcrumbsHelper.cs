using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Base;
using UI.Modules.Breadcrumbs.Models;
using UI.Modules.Breadcrumbs.Views;

namespace UI.Helpers
{
    public class BreadcrumbsHelper : HelperBase
    {
        public BreadcrumbsHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Path to breadcrumbs view
        /// </summary>
        private const string BreadcrumbsViewPath = "~/views/modules/breadcrumbs/breadcrumbs.cshtml";

        /// <summary>
        /// Gets breadcrumbs HTML
        /// </summary>
        /// <param name="items">Items</param>
        /// <returns>Breadcrumbs</returns>
        public IHtmlString GetBreadcrumbs(IList<BreadcrumbItem> items)
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return this.WebViewPage.Html.Partial(BreadcrumbsViewPath, new BreadcrumbsView()
            {
                Breadcrumbs = items
            });
        }
    }
}
