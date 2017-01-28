using System.Web.Mvc;
using Core.Loc;

namespace UI.RazorExtensions
{
    public abstract class UIHelpersWebViewPage<T> : WebViewPage<T> where T : class
    {
        public IUIHelpers Helpers { get; private set; }

        public override void InitHelpers()
        {
            base.InitHelpers();

            // pass web view page to all helper classes as argument
            var webParameter = new ConstructorParameter("webViewPage", this);

            Helpers = KernelProvider.Get<IUIHelpers>(webParameter);
        }
    }
}
