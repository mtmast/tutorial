using EmailService.Configuration;
using EmailService.Models;
using EmailService.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ResetPwd.Models;
using System.Diagnostics;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly IWebHostEnvironment _env;

    public MailService(IOptions<MailSettings> mailSettingsOptions, IWebHostEnvironment env)
    {
        _mailSettings = mailSettingsOptions.Value;
        _env = env;
    }

    //Send Normal Email
    public bool SendMail(MailData mailData)
    {
        try
        {
            using (var emailMessage = new MimeMessage())
            {
                emailMessage.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
                emailMessage.To.Add(new MailboxAddress(mailData.EmailToName, mailData.EmailToId));
                emailMessage.Subject = mailData.EmailSubject;

                var emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (var mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    // Send HTML Email
    public bool SendHTMLMail(HTMLMailData htmlMailData, String resetToken,String resetEmail)
    {
        try
        {
            string webRootPath = _env.WebRootPath;
            string filePath = Path.Combine(webRootPath, "Templates", "ResetEmail.html");
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(htmlMailData.EmailToName, htmlMailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = "Reset Link";
                string emailTemplateText = File.ReadAllText(filePath);
                emailTemplateText = emailTemplateText.Replace("{0}", htmlMailData.EmailToName);
                emailTemplateText = emailTemplateText.Replace("{1}", DateTime.Today.Date.ToShortDateString());
                emailTemplateText = emailTemplateText.Replace("{2}", resetToken);
                emailTemplateText = emailTemplateText.Replace("{3}", resetEmail);

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;
                emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error............ Template Text From OUter==================", ex.Message);
            return false;
        }
    }
}
