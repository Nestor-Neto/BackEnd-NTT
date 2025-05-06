
namespace Ambev.DeveloperEvaluation.WebApi.Features.Register.Create;

public class CreateCustomerRequest
{
    
    public required string CustomerName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public List<CreateBranchRequest> Branches { get; set; } = new List<CreateBranchRequest>();
}

