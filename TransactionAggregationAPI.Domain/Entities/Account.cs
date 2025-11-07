namespace TransactionAggregationAPI.Domain.Entities;

public class Account
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal AvailableBalance { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime OpenedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Customer Customer { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public enum AccountType
{
    Checking = 0,
    Savings = 1,
    Credit = 2,
    Investment = 3,
    Loan = 4,
    Business = 5
}

public enum AccountStatus
{
    Active = 0,
    Inactive = 1,
    Frozen = 2,
    Closed = 3
}