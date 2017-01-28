using System.Collections.Generic;
using UI.Builders.Gallery.Models;

namespace UI.Builders.Gallery.Views
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
