using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;

namespace Ambev.DeveloperEvaluation.Application.Sales.Create;

public class CreateSaleHandler
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerService _customerService;

    public CreateSaleHandler(ISaleRepository saleRepository, ICustomerService customerService)
    {
        _saleRepository = saleRepository;
        _customerService = customerService;
    }

    public async Task<Sale> HandleAsync(CreateSaleCommand command)
    {
        // Valida se o Customer existe usando o serviço
        var customer = await _customerService.ValidateCustomerExistsAsync(command.CustomerId);

        // Converte os DTOs para entidades para validação
        var saleItems = command.Items.Select(item => new SaleItem
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        // Valida as regras de negócio
        CustomerService.ValidateSaleItems(saleItems);

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CustomerId = command.CustomerId,
            CustomerName = customer.CustomerName,
            BranchName = command.BranchName,
            Cancelled = false
        };

        // Agrupa os itens por ProductId para verificar itens idênticos
        var groupedItems = saleItems
            .GroupBy(item => item.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                ProductName = group.First().ProductName,
                TotalQuantity = group.Sum(item => item.Quantity),
                UnitPrice = group.First().UnitPrice
            });

        foreach (var groupedItem in groupedItems)
        {
            decimal discount = CustomerService.CalculateDiscount(groupedItem.TotalQuantity, groupedItem.UnitPrice);

            sale.Items.Add(new SaleItem
            {
                ProductId = groupedItem.ProductId,
                ProductName = groupedItem.ProductName,
                Quantity = groupedItem.TotalQuantity,
                UnitPrice = groupedItem.UnitPrice,
                Discount = discount
            });
        }

        await _saleRepository.AddAsync(sale);
        return sale;
    }
}