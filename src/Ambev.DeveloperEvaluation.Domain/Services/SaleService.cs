using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Interface;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly ILogger<SaleService> _logger;
    private const int MAX_ITEMS_PER_PRODUCT = 20;
    private const int MIN_ITEMS_FOR_DISCOUNT = 4;

    public SaleService(
        IUnitOfWork unitOfWork,
        IMessageBrokerService messageBrokerService,
        ILogger<SaleService> logger)
    {
        _unitOfWork = unitOfWork;
        _messageBrokerService = messageBrokerService;
        _logger = logger;
    }

    public async Task<Sale> CreateSaleAsync(SaleCreateDto saleCreateDto)
    {
        _logger.LogInformation("Iniciando criação de nova venda");
        
        // Validar limite máximo de itens por produto
        foreach (var item in saleCreateDto.Items)
        {
            if (item.Quantity > MAX_ITEMS_PER_PRODUCT)
            {
                throw new BusinessRuleException($"Quantidade máxima de {MAX_ITEMS_PER_PRODUCT} itens excedida para o produto {item.ProductName}");
            }

            // Validar restrição de desconto
            if (item.Quantity < MIN_ITEMS_FOR_DISCOUNT && item.UnitPrice < item.UnitPrice)
            {
                throw new BusinessRuleException($"Não é permitido desconto para quantidades abaixo de {MIN_ITEMS_FOR_DISCOUNT} itens");
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
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };
             
        try
        {
            // Usa o Unit of Work para garantir consistência entre os bancos
            var createdSale = await _unitOfWork.CreateSaleAsync(sale);
            
            // Se chegou aqui, significa que salvou em ambos os bancos
            var totalAmount = sale.Items.Sum(item => item.Quantity * item.UnitPrice);
            await _messageBrokerService.PublishSaleCreatedAsync(sale.Id, sale.CustomerName, totalAmount);
            _logger.LogInformation("Venda {SaleId} criada com sucesso. Valor total: {TotalAmount}", sale.Id, totalAmount);

            return createdSale;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar venda {SaleId}", sale.Id);
            throw;
        }
    }

    public async Task<Sale?> GetSaleAsync(Guid id)
    {
        _logger.LogInformation("Buscando venda com ID: {SaleId}", id);
        var sale = await _unitOfWork.GetSaleAsync(id);
        _logger.LogInformation("Venda {SaleId} {Status}", id, sale == null ? "não encontrada" : "encontrada");
        return sale;
    }

    public async Task<IEnumerable<Sale>> GetSalesAsync(int page, int size)
    {
        _logger.LogInformation("Buscando vendas - Página: {Page}, Tamanho: {Size}", page, size);
        var sales = await _unitOfWork.GetSalesAsync(page, size);
        _logger.LogInformation("Encontradas {Count} vendas", sales.Count());
        return sales;
    }

    public async Task<Sale> CancelSaleAsync(Guid id)
    {
        _logger.LogInformation("Iniciando cancelamento da venda {SaleId}", id);
        
        var sale = await _unitOfWork.GetSaleAsync(id);
        if (sale == null)
        {
            _logger.LogWarning("Tentativa de cancelar venda inexistente: {SaleId}", id);
            throw new BusinessRuleException("Venda não encontrada!");
        }

        if (sale.Cancelled)
        {
            _logger.LogWarning("Tentativa de cancelar venda já cancelada: {SaleId}", id);
            throw new BusinessRuleException("Venda já está cancelada!");
        }

        sale.Cancelled = true;
        await _unitOfWork.UpdateSaleAsync(sale);
        
        // Publica mensagem de venda cancelada
        await _messageBrokerService.PublishSaleCancelledAsync(id, "Venda cancelada pelo usuário");
        _logger.LogInformation("Venda {SaleId} cancelada com sucesso", id);

        return sale;
    }

    public async Task<int> GetTotalSalesCountAsync()
    {
        _logger.LogInformation("Buscando total de vendas");
        var count = await _unitOfWork.GetTotalSalesCountAsync();
        _logger.LogInformation("Total de vendas: {Count}", count);
        return count;
    }
}