namespace TransactionAggregationAPI.Domain.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public CustomerStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}

public enum CustomerStatus
{
    Active = 0,
    Inactive = 1,
    Suspended = 2,
    Closed = 3
}