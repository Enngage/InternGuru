
using System;

namespace UI.Builders.InfoMessage.Models
{
    public class InfoMessageProcessClosableMessageQuery
    {
        public string MessageID { get; set; }
        public DateTime ClosedUntil { get; set; }
        public bool RememberClosed { get; set; }
    }
}
