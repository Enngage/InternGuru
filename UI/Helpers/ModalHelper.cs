using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Base;
using UI.Modules.Modal.Models;

namespace UI.Helpers
{
    public class ModalHelper : HelperBase
    {
        public ModalHelper(WebViewPage webViewPage) : base(webViewPage) { }

        private const string ModalViewPath = "~/Views/Modules/Modal/Modal.cshtml";

        public IHtmlString RenderDeleteModal(string elementClass, string title, string message)
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return this.WebViewPage.Html.Partial(ModalViewPath, new ModalConfig()
            {
                ElementClass = elementClass,
                Title = title,
                Message = message,
                HeaderIconClass = "remove",
                Buttons = new List<ModalButton>()
                {
                    new ModalButton()
                    {
                        CssClass = "red cancel inverted",
                        Text = "Ne",
                        IconClass = "remove"
                    },
                    new ModalButton()
                    {
                        CssClass = "green ok inverted",
                        Text = "Ano",
                        IconClass = "checkmark"
                    },
                }
            });
        }
    }
}
