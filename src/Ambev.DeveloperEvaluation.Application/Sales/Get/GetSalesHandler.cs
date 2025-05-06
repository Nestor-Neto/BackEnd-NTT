using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.Get;

public class GetSalesHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesHandler(ISaleRepository repository)
        {
            _saleRepository = repository;
        }

        public async Task<GetSalesResult> HandleAsync(GetSalesQuery query)
        {
            var sales = await _saleRepository.GetAllAsync(query.Page, query.Size);
            return new GetSalesResult(sales);
        }
    }