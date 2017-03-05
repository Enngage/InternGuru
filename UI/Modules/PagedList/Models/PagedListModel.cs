
using PagedList;

namespace UI.Modules.PagedList.Models
{
    public class PagedListModel
    {
        public IPagedList PagedList { get; set; }
        public bool AddMargin { get; set; } = true;
        public int StandardPagesCount { get; set; }
        public int SimplePagesCount { get; set; }
    }
}
