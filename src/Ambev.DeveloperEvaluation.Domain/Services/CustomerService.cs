using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
namespace Ambev.DeveloperEvaluation.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerDto customerDto)
    {
        // Gera um novo CustomerId
        var customerId = Guid.NewGuid();

        var customer = new Customer
        {
            CustomerId = customerId,
            CustomerName = customerDto.CustomerName,
            Email = customerDto.Email,
            Phone = customerDto.Phone,
            Branches = customerDto.Branches.Select(branch => new Branch
            {
                BranchId = Guid.NewGuid(), // Gera um novo BranchId
                BranchName = branch.BranchName,
                Email = branch.Email,
                Phone = branch.Phone,
                IsBranch = branch.IsBranch,
                CustomerId = customerId
            }).ToList()
        };

        return await _customerRepository.AddAsync(customer);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(int page, int size)
    {
        return await _customerRepository.GetAllAsync(page, size);
    }

    public async Task<int> GetTotalCustomersCountAsync()
    {
        return await _customerRepository.GetTotalCustomersCountAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Validates if the Customer exists and returns its data
    /// </summary>
    /// <param name="customerId">ID of the Customer to be validated</param>
    /// <returns>Customer data if found</returns>
    /// <exception cref="BusinessRuleException">Thrown when the Customer is not found</exception>
    public async Task<Customer> ValidateCustomerExistsAsync(Guid customerId)
    {
        var customer = await GetCustomerByIdAsync(customerId);
        if (customer == null)
            throw new BusinessRuleException("Customer not found, please register and provide ID!");

        return customer;
    }

    /// <summary>
    /// Validates business rules for sale items
    /// </summary>
    /// <param name="items">List of sale items</param>
    /// <exception cref="BusinessRuleException">Thrown when any business rule is violated</exception>
    public static void ValidateSaleItems(IEnumerable<SaleItem> items)
    {
        // Groups items by ProductId to check total quantity per product
        var groupedItems = items
            .GroupBy(item => item.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                ProductName = group.First().ProductName,
                TotalQuantity = group.Sum(item => item.Quantity)
            });

        foreach (var groupedItem in groupedItems)
        {
            // Rule: Cannot sell more than 20 identical items
            if (groupedItem.TotalQuantity > 20)
                throw new BusinessRuleException($"Invalid quantity for product '{groupedItem.ProductName}', above the permitted amount of 20 items.");

            // Basic validation: quantity must be greater than zero
            if (groupedItem.TotalQuantity <= 0)
                throw new BusinessRuleException($"Invalid quantity for product '{groupedItem.ProductName}', quantity must be greater than 0.");
        }
    }

    /// <summary>
    /// Calculates discount based on quantity of identical items
    /// </summary>
    /// <param name="quantity">Total quantity of identical items</param>
    /// <param name="unitPrice">Unit price of the item</param>
    /// <returns>Discount value to be applied</returns>
    public static decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        // Rule: Purchases below 4 items cannot have discount
        if (quantity < 4)
            return 0m;

        // Rule: Purchases between 10 and 20 identical items have 20% discount
        if (quantity >= 10 && quantity <= 20)
            return unitPrice * quantity * 0.20m;

        // Rule: Purchases above 4 identical items have 10% discount
        if (quantity >= 4 && quantity < 10)
            return unitPrice * quantity * 0.10m;

        return 0m;
    }
}

