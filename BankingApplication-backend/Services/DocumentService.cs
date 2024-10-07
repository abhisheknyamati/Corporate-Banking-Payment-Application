using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepo _documentRepo;
        public DocumentService(IDocumentRepo documentRepo)
        {
            _documentRepo = documentRepo;
        }
        public async Task UpdateOrAddDocumentAsync(Document document)
        {
            await _documentRepo.UpdateOrAddDocumentAsync(document);
        }
        public async Task AddDocumentAsync(Document document)
        {
            await _documentRepo.AddDocumentAsync(document); 
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _documentRepo.GetDocumentByIdAsync(id);
        }
    }
}
