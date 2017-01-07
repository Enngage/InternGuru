

using UI.Builders.Shared.Forms;

namespace UI.Base
{
    public abstract class BaseForm
    {
        private FormResult _formResult;

        /// <summary>
        /// Result of the form
        /// </summary>
        public FormResult FormResult
        {
            get
            {
                if (_formResult == null)
                {
                    _formResult = new FormResult()
                    {
                        IsSuccess = false
                    };
                }
                return _formResult;
            }
        }
    }
}
