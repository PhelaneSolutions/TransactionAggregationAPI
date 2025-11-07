using System.ComponentModel.DataAnnotations;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Application.DTOs;

public class AccountDto
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
}

public class CreateAccountDto
{
    [Required(ErrorMessage = "Customer ID is required")]
    [StringLength(50, ErrorMessage = "Customer ID cannot exceed 50 characters")]
    public string CustomerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Account name must be between 3 and 100 characters")]
    public string AccountName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account type is required")]
    [EnumDataType(typeof(AccountType), ErrorMessage = "Invalid account type")]
    public AccountType Type { get; set; }

    [Required(ErrorMessage = "Currency is required")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters (e.g., USD, EUR)")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be 3 uppercase letters (e.g., USD, EUR)")]
    public string Currency { get; set; } = string.Empty;

    [Range(-1000000, 1000000, ErrorMessage = "Initial balance must be between -1,000,000 and 1,000,000")]
    public decimal InitialBalance { get; set; }
}