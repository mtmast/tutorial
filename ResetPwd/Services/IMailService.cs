using EmailService.Models;
using ResetPwd.Models;

namespace EmailService.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
        bool SendHTMLMail(HTMLMailData htmlMailData, String token, DateTime expiry);
    }
}

