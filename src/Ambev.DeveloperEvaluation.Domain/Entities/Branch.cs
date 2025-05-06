namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Branch
{
    public Guid BranchId { get; set; } = Guid.Empty;
    public string? BranchName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool? IsBranch { get; set; }
    public Guid? CustomerId { get; set; }
}
