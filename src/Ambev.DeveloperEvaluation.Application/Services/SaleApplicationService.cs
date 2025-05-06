using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Application.Sales.Create;
using Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleApplicationService
    {
        private readonly ISaleService _saleService;
        private readonly IMessageBrokerService _messageBrokerService;

        public SaleApplicationService(ISaleService saleService, IMessageBrokerService messageBrokerService)
        {
            _saleService = saleService;
            _messageBrokerService = messageBrokerService;
        }

        public async Task<Sale> CreateSaleAsync(CreateSaleCommand command)
        {
            var saleCreateDto = new SaleCreateDto
            {
                CustomerName = command.CustomerName,
                BranchName = command.BranchName,
                Items = command.Items.Select(item => new SaleItemCreateDto
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            var sale = await _saleService.CreateSaleAsync(saleCreateDto);
            
            // Publica mensagem de venda criada
            var totalAmount = sale.Items.Sum(item => item.Quantity * item.UnitPrice);
            await _messageBrokerService.PublishSaleCreatedAsync(sale.Id, sale.CustomerName, totalAmount);

            return sale;
        }

        public async Task<Sale?> GetSaleAsync(Guid id)
        {
            return await _saleService.GetSaleAsync(id);
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(int page, int size)
        {
            return await _saleService.GetSalesAsync(page, size);
        }

        public async Task<int> GetTotalSalesCountAsync()
        {
            return await _saleService.GetTotalSalesCountAsync();
        }

        public async Task CancelSaleAsync(Guid id)
        {
            await _saleService.CancelSaleAsync(id);
            await _messageBrokerService.PublishSaleCancelledAsync(id, "Venda cancelada pelo usuário");
        }
    }
}
