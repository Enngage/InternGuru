
using System;

namespace UI.Builders.Master.Views
{
    public class ErrorView : MasterView
    {
        public string ErrorMessage { get; set; }
        public string Stacktrace { get; set; }
        public string InnerException { get; set; }

        public ErrorView(Exception ex)
        {
            this.ErrorMessage = ex.Message;
            this.Stacktrace = ex.StackTrace;

            if (ex.InnerException != null)
            {
                this.InnerException = ex.InnerException.Message;
            }
        }
    }
}
