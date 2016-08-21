using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace UI.ModelState
{
    public class ModelStateWrapper : IModelState
    {

        #region Variables

        private ModelStateDictionary modelState;

        #endregion

        #region Constructor

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        #endregion

        #region IValidationDictionary Members

        public void AddError(string key, string errorMessage)
        {
            this.modelState.AddModelError(key, errorMessage);
        }

        public void AddError(string errorMessage)
        {
            this.modelState.AddModelError(string.Empty, errorMessage);
        }

        public bool IsValid
        {
            get { return this.modelState.IsValid; }
        }

        public void AddErrors(IEnumerable<String> errorMessages)
        {
            foreach (var error in errorMessages)
            {
                this.modelState.AddModelError("", error);
            }
        }

        #endregion
    }
}
