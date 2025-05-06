using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Get;
    public class GetSalesResult
    {
        public IEnumerable<Sale> Sales { get; set; }

        public GetSalesResult(IEnumerable<Sale> sales)
        {
            Sales = sales;
        }
    }
