
using UI.Builders.Master.Models;

namespace UI.Builders.Master.Views
{
    public class MasterView
    {
        public MasterModel Master { get; set; }
        public MasterMetadata Metadata { get; set; } = new MasterMetadata();
    }
}
