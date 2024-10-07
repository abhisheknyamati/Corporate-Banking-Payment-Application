using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IDownloadService
    {
        Task<Download> GetDocumentByIdAsync(int id);
    }
}
