
using System.Threading.Tasks;

namespace EmailProvider
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(IEmail email);
        void SendEmail(IEmail email);
    }
}
