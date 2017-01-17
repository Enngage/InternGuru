﻿using System.Web.Mvc;
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

        public UIHelpers(
            WebViewPage webViewPage
            )
        {
            var webParameter = new ConstructorParameter("webViewPage", webViewPage);

            CountryHelper = KernelProvider.Get<CountryHelper>(webParameter);
            UrlHelper = KernelProvider.Get<UrlHelper>(webParameter);
            HtmlHelper = KernelProvider.Get<HtmlHelper>(webParameter);
            ImageHelper = KernelProvider.Get<ImageHelper>(webParameter);
            UserHelper = KernelProvider.Get<UserHelper>(webParameter);
            PagerHelper = KernelProvider.Get<PagerHelper>(webParameter);
        }
    }
}
