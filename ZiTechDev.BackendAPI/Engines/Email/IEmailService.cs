using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Email;

namespace ZiTechDev.BackendAPI.Engines.Email
{
    public interface IEmailService
    {
        Task<bool> SendAsync(EmailItem emailItem);
        Task<List<EmailItem>> ReceiveAsync(int maxCount = 10);
    }
}
