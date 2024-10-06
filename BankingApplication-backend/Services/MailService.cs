using BankingApplication_backend.Models;
using System.Net.Mail;
using System.Net;

namespace BankingApplication_backend.Services
{
    public class MailService:IMailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromPassword;

        public MailService(IConfiguration configuration)
        {
            _smtpServer = configuration["SmtpSettings:Server"];
            _smtpPort = int.Parse(configuration["SmtpSettings:Port"]);
            _fromEmail = configuration["SmtpSettings:FromEmail"];
            _fromPassword = configuration["SmtpSettings:FromPassword"];
        }

        public void SendMail(MailData mailData)
        {

            using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = mailData.EmailSubject,
                    Body = mailData.EmailBody,
                    IsBodyHtml = false // Set to true if you're sending HTML emails
                };
                mailMessage.To.Add(mailData.EmailTo);

                smtpClient.Send(mailMessage);
            }
        }
    }
}
