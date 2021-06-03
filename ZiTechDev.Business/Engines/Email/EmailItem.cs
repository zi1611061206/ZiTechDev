using System;
using System.Collections.Generic;
using System.Text;

namespace ZiTechDev.Business.Engines.Email
{
    public class EmailItem
    {
        public List<EmailBase> Senders { get; set; } = new List<EmailBase>();
        public List<EmailBase> Receivers { get; set; } = new List<EmailBase>();
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
