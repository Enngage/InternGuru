using PagedList;
using UI.Builders.System.Models;

namespace UI.Builders.System.Views
{
    public class SystemEmailLogView : SystemMasterView
    {
        public IPagedList<SystemEmailModel> Emails { get; set; }
    }
}
