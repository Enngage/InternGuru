using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using PagedList;
using UI.Base;
using UI.Modules.PagedList.Models;

namespace UI.Helpers
{
    public class PagerHelper: HelperBase
    {
        public PagerHelper(WebViewPage webViewPage) : base(webViewPage) { }

        private const int MobilePagesCount = 0;
        private const int StandardPagesCount = 9;

        private const string PagedListView = "~/views/modules/PagedList/PagedList.cshtml";

        public IHtmlString GetPager(IPagedList pagedList, bool addMargin = true)
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return WebViewPage.Html.Partial(PagedListView, new PagedListModel()
            {
                PagedList = pagedList,
                AddMargin = addMargin,
                SimplePagesCount = MobilePagesCount,
                StandardPagesCount = StandardPagesCount
            });
        }
    }
}
