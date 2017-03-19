using System.Web.Mvc;
using Core.Loc;
using UI.Helpers;
using HtmlHelper = UI.Helpers.HtmlHelper;
using UrlHelper = UI.Helpers.UrlHelper;

namespace UI.RazorExtensions
{
    public class UIHelpers : IUIHelpers
    {
        public CountryHelper CountryHelper { get; }
        public HtmlHelper HtmlHelper { get; }
        public ImageHelper ImageHelper { get; }
        public UserHelper UserHelper { get; }
        public UrlHelper UrlHelper { get; }
        public PagerHelper PagerHelper { get; }
        public ScriptHelper ScriptHelper { get; }
        public MessageHelper MessageHelper { get; }
        public InputHelper InputHelper { get; }
        public ModalHelper ModalHelper { get; }
        public ActionHelper ActionHelper { get; }
        public TextHelper TextHelper { get; }
        public BreadcrumbsHelper BreadcrumbsHelper { get; }
        public UploadHelper UploadHelper { get; }
        public HeaderHelper HeaderHelper { get; }
        public GoogleMapHelper GoogleMapHelper { get; }
        public SocialHelper SocialHelper { get; }

        public UIHelpers(WebViewPage webViewPage)
        {
            var webParameter = new ConstructorParameter("webViewPage", webViewPage);

            CountryHelper = KernelProvider.Get<CountryHelper>(webParameter);
            UrlHelper = KernelProvider.Get<UrlHelper>(webParameter);
            HtmlHelper = KernelProvider.Get<HtmlHelper>(webParameter);
            ImageHelper = KernelProvider.Get<ImageHelper>(webParameter);
            UserHelper = KernelProvider.Get<UserHelper>(webParameter);
            PagerHelper = KernelProvider.Get<PagerHelper>(webParameter);
            ScriptHelper = KernelProvider.Get<ScriptHelper>(webParameter);
            MessageHelper = KernelProvider.Get<MessageHelper>(webParameter);
            InputHelper = KernelProvider.Get<InputHelper>(webParameter);
            ModalHelper = KernelProvider.Get<ModalHelper>(webParameter);
            ActionHelper = KernelProvider.Get<ActionHelper>(webParameter);
            TextHelper = KernelProvider.Get<TextHelper>(webParameter);
            BreadcrumbsHelper = KernelProvider.Get<BreadcrumbsHelper>(webParameter);
            UploadHelper = KernelProvider.Get<UploadHelper>(webParameter);
            HeaderHelper = KernelProvider.Get<HeaderHelper>(webParameter);
            GoogleMapHelper = KernelProvider.Get<GoogleMapHelper>(webParameter);
            SocialHelper = KernelProvider.Get<SocialHelper>(webParameter);
        }
    }
}
