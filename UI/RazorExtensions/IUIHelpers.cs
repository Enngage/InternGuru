
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
    }
}
