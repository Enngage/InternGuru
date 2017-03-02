using System.Web.Mvc;
using UI.Builders.FineUpload.Models;


namespace UI.Builders.FineUpload.Binders
{
    public class FIneUploadBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;
            var formUpload = request.Files.Count > 0;

            // find filename
            var xFileName = request.Headers["X-File-Name"];

            var httpPostedFileBase = request.Files[0];
            if (httpPostedFileBase != null)
            {
                var formFilename = formUpload ? httpPostedFileBase.FileName : null;

                var upload = new FineUploadModel
                {
                    Filename = xFileName ?? formFilename,
                    File = httpPostedFileBase
                };

                return upload;
            }
            return null;
        }
    }
}
