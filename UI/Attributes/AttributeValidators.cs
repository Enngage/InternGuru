using System.Web.Mvc;

namespace UI.Attributes
{
    public class ValidIntegerValidator : System.Web.Mvc.DataAnnotationsModelValidator<ValidInteger>
    {
        public ValidIntegerValidator(System.Web.Mvc.ModelMetadata metadata, ControllerContext context, ValidInteger attribute)
            : base(metadata, context, attribute)
        {
            if (!attribute.IsValid(context.HttpContext.Request.Form[metadata.PropertyName]))
            {
                var propertyName = metadata.PropertyName;
                context.Controller.ViewData.ModelState[propertyName].Errors.Clear();
                context.Controller.ViewData.ModelState[propertyName].Errors.Add(attribute.ErrorMessage);
            }
        }
    }
}
