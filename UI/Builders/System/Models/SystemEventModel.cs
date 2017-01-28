using System;

namespace UI.Builders.System.Models
{
    public class SystemEventModel
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public string Url { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerException { get; set; }
        public string Stacktrace { get; set; }
        public string ApplicationUserName { get; set; }
    }
}
