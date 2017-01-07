using System.Collections.Generic;
using System.Web.Mvc;

namespace UI.ModelState
{
    public class ModelStateWrapper : IModelState
    {

        #region Variables

        private readonly ModelStateDictionary _modelState;

        #endregion

        #region Constructor

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        #endregion

        #region IValidationDictionary Members

        public void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }

        public void AddError(string errorMessage)
        {
            _modelState.AddModelError(string.Empty, errorMessage);
        }

        public bool IsValid
        {
            get { return _modelState.IsValid; }
        }

        public void AddErrors(IEnumerable<string> errorMessages)
        {
            foreach (var error in errorMessages)
            {
                _modelState.AddModelError("", error);
            }
        }

        #endregion
    }
}
