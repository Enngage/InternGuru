
using UI.Builders.Shared;

namespace UI.Builders.Master.Models
{
    public class MasterModel
    {
        public ICurrentUser CurrentUser { get; set; }
        public ICurrentCompany CurrentCompany { get; set; }
    }
}
