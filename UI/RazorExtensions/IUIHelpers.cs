
using UI.Helpers;
using HtmlHelper = UI.Helpers.HtmlHelper;
using UrlHelper = UI.Helpers.UrlHelper;

namespace UI.RazorExtensions
{
    public interface IUIHelpers
    {
        CountryHelper CountryHelper { get; }
        HtmlHelper HtmlHelper { get; }
        ImageHelper ImageHelper { get; }
        UserHelper UserHelper { get; }
        UrlHelper UrlHelper { get; }
        PagerHelper PagerHelper { get; }
        ScriptHelper ScriptHelper { get; }
        MessageHelper MessageHelper { get; }
        InputHelper InputHelper { get; }
        ModalHelper ModalHelper { get; }
        ActionHelper ActionHelper { get; }
        TextHelper TextHelper { get; }
        BreadcrumbsHelper BreadcrumbsHelper { get; }
        UploadHelper UploadHelper { get; }
        HeaderHelper HeaderHelper { get; }
        GoogleMapHelper GoogleMapHelper { get; }
    }
}
