using UI.Builders.Shared.Models;

namespace UI.Builders.Master.Models
{
    public class MasterModel
    {
        public ICurrentUser CurrentUser { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
        public IStatusBox StatusBox { get; set; }
        public IUiHeader UiHeader { get; set; }
    }
}
