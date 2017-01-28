using System.Web;
using UI.Base;

namespace UI.Builders.Auth.Forms
{
    public class AuthAvatarUploadForm : BaseForm
    {
        public HttpPostedFileBase Avatar { get; set; }
    }
}
