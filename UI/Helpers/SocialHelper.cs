using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Base;
using UI.Modules.Facebook.Models;

namespace UI.Helpers
{
    public class SocialHelper : HelperBase
    {
        public SocialHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Path to facebook share view
        /// </summary>
        private const string FacebokShareViewPath = "~/views/modules/facebook/facebookShare.cshtml";

        /// <summary>
        /// Gets faceboko share button 
        /// </summary>
        /// <param name="shareUrl">URL to share</param>
        /// <param name="buttonText">Button text</param>
        /// <returns>Facebook share button</returns>
        public IHtmlString GetFacebookShareButton(string shareUrl, string buttonText = "Sdílet na Facebook")
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return this.WebViewPage.Html.Partial(FacebokShareViewPath, new FacebookShareModel()
            {
                ShareUrl = shareUrl,
                ShareButtonText = buttonText
            });
        }
    }
}
