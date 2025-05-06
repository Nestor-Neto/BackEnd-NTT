namespace Ambev.DeveloperEvaluation.Application.Sales.Create;
    public class CreateSaleItemDto
    {
        public Guid ProductId { get; set; } = Guid.Empty;
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }