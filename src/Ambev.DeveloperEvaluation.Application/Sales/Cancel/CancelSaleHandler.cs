
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;

public class CancelSaleHandler
    {
        private readonly ISaleRepository _saleRepository;
         

        public CancelSaleHandler(ISaleRepository repository)
        {
            _saleRepository = repository;
        }

        public async Task<CancelSaleResult> HandleAsync(CancelSaleCommand command)
        {
            var sale = await _saleRepository.GetByIdAsync(command.SaleId)
                ?? throw new InvalidOperationException("Sale not found.");

            sale.Cancelled = true;

            await _saleRepository.UpdateAsync(sale);

            return new CancelSaleResult(sale.Id, sale.Cancelled);
        }
    }
