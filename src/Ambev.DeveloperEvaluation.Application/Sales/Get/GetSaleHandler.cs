using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Get;

public class GetSaleHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSaleHandler(ISaleRepository repository)
        {
            _saleRepository = repository;
        }

        public async Task<GetSaleResult?> HandleAsync(GetSaleQuery query)
        {
            var sale = await _saleRepository.GetByIdAsync(query.SaleId);
            if (sale is null) return null;

            return new GetSaleResult
            {
                Id = sale.Id,
                Date = sale.Date,
                CustomerName = sale.CustomerName,
                BranchName = sale.BranchName,
                TotalAmount = sale.TotalAmount,
                Cancelled = sale.Cancelled,
                Items = sale.Items
            };
        }
    }