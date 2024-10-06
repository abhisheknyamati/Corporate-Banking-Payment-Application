using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IDocumentService
    {
        Task AddDocumentAsync(Document document);
        Task<Document> GetDocumentByIdAsync(int id);
        
    }
}
