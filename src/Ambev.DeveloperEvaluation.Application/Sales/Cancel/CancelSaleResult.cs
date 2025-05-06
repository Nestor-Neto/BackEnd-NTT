namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;
public class CancelSaleResult
    {
        public Guid SaleId { get; set; }
        public bool Cancelled { get; set; }

        public CancelSaleResult(Guid saleId, bool cancelled)
        {
            SaleId = saleId;
            Cancelled = cancelled;
        }
    }