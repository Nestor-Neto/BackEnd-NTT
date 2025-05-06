namespace Ambev.DeveloperEvaluation.Domain.Entities.DataTransferObjects;

public class CreateBranchDto
{
    public string BranchName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsBranch { get; set; }
}
