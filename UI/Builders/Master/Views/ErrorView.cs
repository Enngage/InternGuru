
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
            ErrorMessage = ex.Message;
            Stacktrace = ex.StackTrace;

            if (ex.InnerException != null)
            {
                InnerException = ex.InnerException.Message;
            }
        }
    }
}
