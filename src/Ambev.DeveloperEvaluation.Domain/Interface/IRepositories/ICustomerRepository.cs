using Ambev.DeveloperEvaluation.Domain.Entities;
namespace Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer);
    Task<IEnumerable<Customer>> GetAllAsync(int page, int size);
    Task<int> GetTotalCustomersCountAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Branch?> GetBranchByIdAsync(Guid branchId);
}

