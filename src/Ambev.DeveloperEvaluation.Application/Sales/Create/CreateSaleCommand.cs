namespace Ambev.DeveloperEvaluation.Application.Sales.Create;

public class CreateSaleCommand
{
    public required Guid CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public Guid BranchId { get; set; } = Guid.Empty;
    public required string BranchName { get; set; }
    public required List<CreateSaleItemDto> Items { get; set; }
}
