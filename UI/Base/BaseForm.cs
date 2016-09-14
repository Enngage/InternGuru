

using UI.Builders.Shared.Forms;

namespace UI.Base
{
    public abstract class BaseForm
    {
        private FormResult formResult;

        /// <summary>
        /// Result of the form
        /// </summary>
        public FormResult FormResult
        {
            get
            {
                if (formResult == null)
                {
                    formResult = new FormResult()
                    {
                        IsSuccess = false
                    };
                }
                return formResult;
            }
        }
    }
}
