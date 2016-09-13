using System.Web;
using UI.Abstract;

namespace UI.Builders.Auth.Forms
{
    public class AuthAvatarUploadForm : BaseForm
    {
        public HttpPostedFileBase Avatar { get; set; }
    }
}
