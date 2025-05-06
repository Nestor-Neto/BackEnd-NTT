using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
    public interface ISaleRepository
    {
        Task<Sale> AddAsync(Sale sale);
        Task<Sale?> GetByIdAsync(Guid saleId);
        Task<IEnumerable<Sale>> GetAllAsync(int page, int size);
        Task UpdateAsync(Sale sale);
        Task<int> GetTotalCountAsync();
        Task<Sale> CancelSaleAsync(Guid id);
    }