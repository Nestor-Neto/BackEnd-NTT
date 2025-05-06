namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Customer
{
    public Guid CustomerId { get; set; } = Guid.Empty;
    public required string CustomerName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
