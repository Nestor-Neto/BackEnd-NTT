
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Create;

public class CreateSaleRequest
{
    public required Guid CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public required Guid branchId { get; set; }
    public required string BranchName { get; set; }
    public List<CreateSaleItemRequest> Items { get; set; } = new  List<CreateSaleItemRequest>();
    
}
