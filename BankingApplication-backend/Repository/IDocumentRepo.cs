using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IDocumentRepo
    {
        Task AddDocumentAsync(Document document);
        Task<Document> GetDocumentByIdAsync(int id);
    }
}
