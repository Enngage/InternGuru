
namespace UI.Builders.Master.Models
{
    public class MasterSocial
    {
        /// <summary>
        /// Apple metadata
        /// </summary>
        public MasterAppleMetadata AppleMetadata { get; set; } = new MasterAppleMetadata();

        /// <summary>
        /// Google metadata
        /// </summary>
        public MasterGooglePlusMetadata GoogleMetadata { get; set; } = new MasterGooglePlusMetadata();

        /// <summary>
        /// OpenGraph metadata
        /// </summary>
        public MasterOpenGraphMetadata OpenGraphMetadata { get; set; } = new MasterOpenGraphMetadata();

    }
}
