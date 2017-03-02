using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using UI.Base;
using UI.Modules.FineUploader.Models;

namespace UI.Helpers
{
    public class UploadHelper : HelperBase
    {
        public UploadHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Path to uploader view
        /// </summary>
        private const string FineUploaderViewPath = "~/views/modules/fineuploader/fineuploader.cshtml";

        public IHtmlString RenderFineUploader(string elementID, string uploadFileActionUrl, IList<string> allowedExtensions, int limitFilesCount, int maxFileSizeBytes, string refreshImagesElementClass = null)
        {
            // ReSharper disable once Mvc.PartialViewNotResolved
            return WebViewPage.Html.Partial(FineUploaderViewPath, new FineUploaderConfig(elementID, uploadFileActionUrl, allowedExtensions, limitFilesCount, maxFileSizeBytes, refreshImagesElementClass));
        }
    }
}
