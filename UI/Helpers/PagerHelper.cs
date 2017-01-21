using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using UI.Base;

namespace UI.Helpers
{
    public class PagerHelper: HelperBase
    {
        public PagerHelper(WebViewPage webViewPage) : base(webViewPage) { }

        private const int MobilePagesCount = 0;
        private const int StandardPagesCount = 9;


        /// <summary>
        /// Gets pager for current controller and actions.
        /// Pager is hidden if there are no items
        /// </summary>
        /// <param name="pagedList">Paged list</param>
        /// <param name="generatePageUrl">function to get page url. Example: 
        /// page => WebViewPage.Url.Action(action, controller, new { page }
        /// </param>
        /// <returns>HTML pager</returns>
        public IHtmlString GetPager(IPagedList pagedList, Func<int, string> generatePageUrl)
        {
            if (pagedList == null)
            {
                return null;
            }

            if (pagedList.TotalItemCount == 0)
            {
                return null;
            }

            var sb = new StringBuilder();

            var standardPager = WebViewPage.Html.PagedListPager(pagedList,
                generatePageUrl,
                new PagedListRenderOptions()
                {
                    Display = PagedListDisplayMode.IfNeeded,
                    MaximumPageNumbersToDisplay = StandardPagesCount,
                    DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded,
                    DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded,
                    DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                    DisplayPageCountAndCurrentLocation = false,
                });

            var mobilePager = WebViewPage.Html.PagedListPager(pagedList,
                generatePageUrl,
                new PagedListRenderOptions()
                {
                    MaximumPageNumbersToDisplay = MobilePagesCount,
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded,
                    Display = PagedListDisplayMode.IfNeeded,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                    DisplayPageCountAndCurrentLocation = false,
                    LinkToNextPageFormat = "Předchozí",
                    LinkToPreviousPageFormat = "Další"
                });

            // build pager html
            sb.AppendLine("<div class=\"w-mobile-show\">");
            sb.Append(mobilePager);
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"w-tablet-show w-computer-show\">");
            sb.Append(standardPager);
            sb.AppendLine("</div>");

            return WebViewPage.Html.Raw(sb.ToString());
        }

        /// <summary>
        /// Gets pager for current controller and actions.
        /// Pager is hidden if there are no items
        /// </summary>
        /// <param name="pagedList">Paged list</param>
        /// <returns>HTML pager</returns>
        public IHtmlString GetPager(IPagedList pagedList)
        {
            var currentAction = WebViewPage.ViewContext.RouteData.Values["action"]?.ToString();
            var currentController = WebViewPage.ViewContext.RouteData.Values["controller"]?.ToString();

            if (string.IsNullOrEmpty(currentController))
            {
                throw new NullReferenceException(nameof(currentController));
            }

            if (string.IsNullOrEmpty(currentAction))
            {
                throw new NullReferenceException(nameof(currentAction));
            }

            return GetPager(pagedList, page => WebViewPage.Url.Action(currentAction, currentController, new { page }));
        }
    }
}
