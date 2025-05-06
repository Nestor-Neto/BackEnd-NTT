using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

namespace Ambev.DeveloperEvaluation.Domain.Interface.IServices;


public interface ISaleService
{
    Task<Sale> CreateSaleAsync(SaleCreateDto dto);
    Task<Sale?> GetSaleAsync(Guid id);
    Task<IEnumerable<Sale>> GetSalesAsync(int page, int size);
    Task<Sale> CancelSaleAsync(Guid id);
    Task<int> GetTotalSalesCountAsync();
}