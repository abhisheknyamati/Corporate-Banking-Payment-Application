using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IMailService
    {
        void SendMail(MailData mailData);
    }
}
