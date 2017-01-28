
using System.Threading.Tasks;

namespace EmailProvider
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(IEmailMessage emailMessage);
        void SendEmail(IEmailMessage emailMessage);
    }
}
