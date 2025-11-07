using System.ComponentModel.DataAnnotations;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Application.DTOs;

public class TransactionDto
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
}

public class CreateTransactionDto
{
    [Required(ErrorMessage = "Customer ID is required")]
    [StringLength(50, ErrorMessage = "Customer ID cannot exceed 50 characters")]
    public string CustomerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account ID is required")]
    [StringLength(50, ErrorMessage = "Account ID cannot exceed 50 characters")]
    public string AccountId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required")]
    [Range(-1000000, 1000000, ErrorMessage = "Amount must be between -1,000,000 and 1,000,000")]
    [RegularExpression(@"^-?\d+(\.\d{1,2})?$", ErrorMessage = "Amount can have maximum 2 decimal places")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Currency is required")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters (e.g., USD, EUR)")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be 3 uppercase letters (e.g., USD, EUR)")]
    public string Currency { get; set; } = string.Empty;

    [Required(ErrorMessage = "Transaction date is required")]
    [DataType(DataType.DateTime)]
    [CustomValidation(typeof(CreateTransactionDto), nameof(ValidateTransactionDate))]
    public DateTime TransactionDate { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 500 characters")]
    public string Description { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Merchant name cannot exceed 100 characters")]
    public string MerchantName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Transaction type is required")]
    [EnumDataType(typeof(TransactionType), ErrorMessage = "Invalid transaction type")]
    public TransactionType Type { get; set; }

    [StringLength(100, ErrorMessage = "Reference number cannot exceed 100 characters")]
    public string ReferenceNumber { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Data source cannot exceed 100 characters")]
    public string DataSource { get; set; } = string.Empty;

    public static ValidationResult? ValidateTransactionDate(DateTime transactionDate, ValidationContext context)
    {
        var now = DateTime.UtcNow;
        var oneYearAgo = now.AddYears(-1);
        var oneMonthInFuture = now.AddMonths(1);

        if (transactionDate < oneYearAgo)
        {
            return new ValidationResult("Transaction date cannot be more than 1 year in the past");
        }

        if (transactionDate > oneMonthInFuture)
        {
            return new ValidationResult("Transaction date cannot be more than 1 month in the future");
        }

        return ValidationResult.Success;
    }
}

public class TransactionSummaryDto
{
    public int TotalTransactions { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AverageAmount { get; set; }
    public Dictionary<TransactionCategory, int> CategoryCounts { get; set; } = new();
    public Dictionary<TransactionCategory, decimal> CategoryAmounts { get; set; } = new();
    public Dictionary<string, int> MonthlyTransactionCounts { get; set; } = new();
    public Dictionary<string, decimal> MonthlyAmounts { get; set; } = new();
}