
using UI.Builders.Master.Enums;
using UI.Builders.Master.Models;

namespace UI.Builders.Master.Views
{
    public class MasterView
    {
        public MasterModel Master { get; set; }
        public MasterMetadata Metadata { get; set; } = new MasterMetadata();

        public Layout Layout { get; set; } = new Layout()
        {
            Type = LayoutTypeEnum.Dark
        };
    }
}
