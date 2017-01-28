using PagedList;
using UI.Builders.System.Models;

namespace UI.Builders.System.Views
{
    public class SystemEmailLogView : SystemAuthView
    {
        public IPagedList<SystemEmailModel> Emails { get; set; }
    }
}
