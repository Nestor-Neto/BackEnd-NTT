using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.Get;
    public class GetSaleResult
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Cancelled { get; set; }
        public IEnumerable<SaleItem> Items { get; set; }
    }