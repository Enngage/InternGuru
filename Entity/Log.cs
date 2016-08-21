using System;

namespace Entity
{
    public class Log : EntityAbstract
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public String Url { get; set; }
        public String ExceptionMessage { get; set; }
        public String InnerException { get; set; }
        public String Stacktrace { get; set; }
        public String ApplicationUserName { get; set; }
    }
}
