namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;

public class CreateSaleItemRequest
{
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
}