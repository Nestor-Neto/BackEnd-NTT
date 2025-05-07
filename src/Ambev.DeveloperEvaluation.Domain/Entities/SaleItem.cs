namespace Ambev.DeveloperEvaluation.Domain.Entities;
    public class SaleItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public decimal TotalAmount => (UnitPrice * Quantity) - Discount;

        public SaleItem()
        {
            ProductName = string.Empty;
        }
    }
