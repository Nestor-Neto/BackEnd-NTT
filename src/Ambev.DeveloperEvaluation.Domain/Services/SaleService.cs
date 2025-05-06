using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBrokerService _messageBrokerService;
    private const int MAX_ITEMS_PER_PRODUCT = 20;
    private const int MIN_ITEMS_FOR_DISCOUNT = 4;

    public SaleService(ISaleRepository saleRepository, IMessageBrokerService messageBrokerService)
    {
        _saleRepository = saleRepository;
        _messageBrokerService = messageBrokerService;
    }

    public async Task<Sale> CreateSaleAsync(SaleCreateDto saleCreateDto)
    {
        // Validar limite máximo de itens por produto
        foreach (var item in saleCreateDto.Items)
        {
            if (item.Quantity > MAX_ITEMS_PER_PRODUCT)
            {
                throw new Exception($"Quantidade máxima de {MAX_ITEMS_PER_PRODUCT} itens excedida para o produto {item.ProductName}");
            }

            // Validar restrição de desconto
            if (item.Quantity < MIN_ITEMS_FOR_DISCOUNT && item.UnitPrice < item.UnitPrice)
            {
                throw new Exception($"Não é permitido desconto para quantidades abaixo de {MIN_ITEMS_FOR_DISCOUNT} itens");
            }
        }

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = saleCreateDto.CustomerName,
            BranchId = Guid.NewGuid(),
            BranchName = saleCreateDto.BranchName,
            Items = saleCreateDto.Items.Select(item => new SaleItem
            {
                ProductId = Guid.NewGuid(),
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };
             
        var createdSale = await _saleRepository.AddAsync(sale);
        
        // Publica mensagem de venda criada
        var totalAmount = sale.Items.Sum(item => item.Quantity * item.UnitPrice);
        await _messageBrokerService.PublishSaleCreatedAsync(sale.Id, sale.CustomerName, totalAmount);

        return createdSale;
    }

    public async Task<Sale?> GetSaleAsync(Guid id)
    {
        return await _saleRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Sale>> GetSalesAsync(int page, int size)
    {
        return await _saleRepository.GetAllAsync(page, size);
    }

    public async Task<Sale> CancelSaleAsync(Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
            throw new Exception("Sale not found!");

        var cancelledSale = await _saleRepository.CancelSaleAsync(id);
        
        // Publica mensagem de venda cancelada
        await _messageBrokerService.PublishSaleCancelledAsync(id, "Venda cancelada pelo usuário");

        return cancelledSale!;
    }

    public async Task<int> GetTotalSalesCountAsync()
    {
        return await _saleRepository.GetTotalCountAsync();
    }
}