namespace TransactionAggregationAPI.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public TransactionStatus Status { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string DataSource { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum TransactionType
{
    Debit = 0,
    Credit = 1,
    Transfer = 2,
    Fee = 3,
    Interest = 4,
    Dividend = 5,
    Refund = 6
}

public enum TransactionCategory
{
    Unknown = 0,
    Food = 1,
    Transportation = 2,
    Entertainment = 3,
    Shopping = 4,
    Bills = 5,
    Healthcare = 6,
    Education = 7,
    Travel = 8,
    Investment = 9,
    Income = 10,
    Transfer = 11,
    Fee = 12,
    Insurance = 13,
    Charity = 14,
    Other = 15
}

public enum TransactionStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Cancelled = 3,
    Processing = 4
}