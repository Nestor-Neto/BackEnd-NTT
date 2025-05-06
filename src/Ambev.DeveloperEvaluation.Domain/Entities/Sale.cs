namespace Ambev.DeveloperEvaluation.Domain.Entities;
public class Sale
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public required string BranchName { get; set; }
        public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
        public bool Cancelled { get; set; }

        public decimal TotalAmount => Items.Sum(i => i.TotalAmount);
    }