using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Interface.IServices
{
    public interface IMongoDBService
    {
        Task SaveSalesAsync(object salesData);
        Task<object> GetSalesAsync(string id);
        Task DeleteSaleAsync(string id);
    }
} 