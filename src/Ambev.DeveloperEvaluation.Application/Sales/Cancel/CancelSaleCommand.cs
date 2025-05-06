namespace Ambev.DeveloperEvaluation.Application.Sales.Cancel;
    public class CancelSaleCommand
    {
        public Guid SaleId { get; set; }

        public CancelSaleCommand(Guid saleId)
        {
            SaleId = saleId;
        }
    }