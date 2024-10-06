using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class DocumentRepo: IDocumentRepo
    {
        private readonly BankingAppDbContext _context;
        public DocumentRepo(BankingAppDbContext context)
        {
            _context = context;
        }

        public async Task AddDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id); 
        }
    }
}
