using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Email;

namespace ZiTechDev.BackendAPI.Engines.Email
{
    public interface IEmailService
    {
        void Send(EmailItem emailItem);
        List<EmailItem> Receive(int maxCount = 10);
    }
}
