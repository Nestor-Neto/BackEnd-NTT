
namespace Ambev.DeveloperEvaluation.Application.Sales.Get;
    public class GetSaleQuery
    {
        public Guid SaleId { get; set; }

        public GetSaleQuery(Guid saleId)
        {
            SaleId = saleId;
        }
    }