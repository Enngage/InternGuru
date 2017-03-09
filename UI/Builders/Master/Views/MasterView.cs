
using UI.Builders.Master.Enums;
using UI.Builders.Master.Models;
using UI.Builders.Shared.Models;
using UI.Modules.Header;

namespace UI.Builders.Master.Views
{
    public class MasterView
    {
        public MasterModel Master { get; set; }
        public MasterMetadata Metadata { get; set; } = new MasterMetadata();
        public IUiHeader Header { get; set; } = new UiHeader();

        public Layout Layout { get; set; } = new Layout()
        {
            Type = LayoutTypeEnum.Dark
        };
    }
}
