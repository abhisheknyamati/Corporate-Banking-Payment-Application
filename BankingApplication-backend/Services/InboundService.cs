using BankingApplication_backend.Data;
using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public class InboundService:IInboundService
    {
        private readonly BankingAppDbContext _context;

        public InboundService(BankingAppDbContext context)
        {
            _context = context;
        }

        public async Task AddInbound(Inbound inbound)
        {
            await _context.InboundOrgs.AddAsync(inbound);
            await _context.SaveChangesAsync();
        }
     

    }
}
