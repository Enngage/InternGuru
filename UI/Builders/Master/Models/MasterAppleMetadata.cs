
using Core.Config;

namespace UI.Builders.Master.Models
{
    public class MasterAppleMetadata
    {
        /// <summary>
        /// Path to apple touch icon
        /// Needs to be converted to Absolute URL
        /// </summary>
        public string AppleTouchIconPath { get; set; } = MetadataConfig.AppleTouchIconPath;
    }
}
