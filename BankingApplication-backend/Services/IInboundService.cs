using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IInboundService
    {
        Task AddInbound(Inbound inbound);
     
    }
}
