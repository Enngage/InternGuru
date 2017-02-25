using System.Collections.Generic;
using UI.Modules.CompanyGallery.Models;

namespace UI.Modules.CompanyGallery.Views
{
    public class GalleryView
    {
        /// <summary>
        /// ID of the element holding gallery
        /// </summary>
        public string GalleryID { get; set; }

        /// <summary>
        /// Collection of gallery images
        /// </summary>
        public IEnumerable<GalleryImage> Images { get; set; }
    }
}
