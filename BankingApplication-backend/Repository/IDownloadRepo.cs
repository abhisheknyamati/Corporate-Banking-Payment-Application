using BankingApplication_backend.Models;
using System.Threading.Tasks;

namespace BankingApplication_backend.Repository
{
    public interface IDownloadRepo
    {
        Task<Download> GetDocumentByIdAsync(int id);
    }
}
