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
        public async Task UpdateOrAddDocumentAsync(Document document)
        {
            var existingDocument = await _context.Documents.Where(d => d.OrganisationId == document.OrganisationId).FirstOrDefaultAsync();

            if (existingDocument != null)
            {
                // Update existing document properties
                existingDocument.FileName = document.FileName;
                existingDocument.FilePath = document.FilePath;
                existingDocument.FileType = document.FileType;
                // Update other properties as necessary
            }
            else
            {
                // Add new document
                await _context.Documents.AddAsync(document);
            }

            await _context.SaveChangesAsync();
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
