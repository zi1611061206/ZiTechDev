using System.Collections.Generic;
using System.Threading.Tasks;
using ZiTechDev.CommonModel.Engines.Email;

namespace ZiTechDev.Api.EmailConfiguration
{
    public interface IEmailService
    {
        Task<bool> SendAsync(EmailItem emailItem);
        Task<List<EmailItem>> ReceiveAsync(int maxCount = 10);
    }
}
