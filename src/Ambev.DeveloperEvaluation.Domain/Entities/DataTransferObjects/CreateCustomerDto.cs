namespace Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

public class CreateCustomerDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<CreateBranchDto> Branches { get; set; } = new();
}
