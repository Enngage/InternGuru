
using Core.Config;

namespace UI.Builders.Master.Models
{
    public class MasterMetadata
    {
        public MasterSocial Social { get; set; } = new MasterSocial();
        public MasterBasicMetadata BasicMetadata { get; set; } = new MasterBasicMetadata();
    }
}
