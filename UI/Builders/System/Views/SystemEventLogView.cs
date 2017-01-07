using PagedList;
using UI.Builders.System.Models;

namespace UI.Builders.System.Views
{
    public class SystemEventLogView : SystemAuthView
    {
        public IPagedList<SystemEventModel> Events { get; set; }
    }
}
