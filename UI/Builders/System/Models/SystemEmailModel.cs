using System;
namespace UI.Builders.System.Models
{
    public class SystemEmailModel
    {
        public int ID { get; set; }
        public Guid Guid { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string HtmlBody { get; set; }
        public bool IsSent { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Sent { get; set; }
        public string Result { get; set; }
    }
}
