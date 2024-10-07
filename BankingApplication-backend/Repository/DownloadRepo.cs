using BankingApplication_backend.Data;
using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public class DownloadRepo:IDownloadRepo
    {
        private readonly BankingAppDbContext _context;

        public DownloadRepo(BankingAppDbContext context)
        {
            _context = context;
        }

        public async Task<Download> GetDocumentByIdAsync(int id)
        {
            // Assuming you have a Documents table in your database
            return await _context.Downloads.FindAsync(id);
        }
    }
}
