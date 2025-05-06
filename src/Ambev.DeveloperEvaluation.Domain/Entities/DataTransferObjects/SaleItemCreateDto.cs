namespace Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;
public class SaleCreateDto
{
    public Guid CustomerId { get; set; } = Guid.Empty;
    public required string CustomerName { get; set; }
    public Guid BranchId { get; set; } = Guid.Empty;
    public required string BranchName { get; set; }
    public required List<SaleItemCreateDto> Items { get; set; }
}

public class SaleItemCreateDto
{
    public required string ProductName { get; set; }
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
}