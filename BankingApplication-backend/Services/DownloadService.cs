using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class DownloadService:IDownloadService
    {
        private readonly IDownloadRepo _documentRepository;

        public DownloadService(IDownloadRepo documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<Download> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetDocumentByIdAsync(id);
        }
    }
}
