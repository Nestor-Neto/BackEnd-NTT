using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<Sale?> GetSaleAsync(Guid id);
        Task<IEnumerable<Sale>> GetSalesAsync(int page, int size);
        Task UpdateSaleAsync(Sale sale);
        Task<int> GetTotalSalesCountAsync();
        Task<bool> CommitAsync();
        Task RollbackAsync();
    }
} 