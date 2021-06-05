using System.Collections.Generic;

namespace ZiTechDev.Api.Engines.Email
{
    public class EmailItem
    {
        public List<EmailBase> Senders { get; set; } = new List<EmailBase>();
        public List<EmailBase> Receivers { get; set; } = new List<EmailBase>();
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
