using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

namespace Ambev.DeveloperEvaluation.Domain.Interface.IServices;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(CreateCustomerDto customerDto);
    Task<IEnumerable<Customer>> GetAllAsync(int page, int size);
    Task<int> GetTotalCustomersCountAsync();
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    Task<Customer> ValidateCustomerExistsAsync(Guid customerId);
} 