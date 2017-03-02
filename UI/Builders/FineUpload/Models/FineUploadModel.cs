using System.Web;

namespace UI.Builders.FineUpload.Models
{
    public class FineUploadModel
    {
        public string Filename { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}
