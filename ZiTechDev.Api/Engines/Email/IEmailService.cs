using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZiTechDev.Api.Engines.Email
{
    public interface IEmailService
    {
        Task<bool> SendAsync(EmailItem emailItem);
        Task<List<EmailItem>> ReceiveAsync(int maxCount = 10);
    }
}
