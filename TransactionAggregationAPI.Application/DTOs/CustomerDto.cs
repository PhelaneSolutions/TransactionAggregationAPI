using System.ComponentModel.DataAnnotations;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Application.DTOs;

public class CustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public CustomerStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AccountDto> Accounts { get; set; } = new();
}

public class CreateCustomerDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "First name can only contain letters, spaces, hyphens, and apostrophes")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Last name can only contain letters, spaces, hyphens, and apostrophes")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in international format (e.g., +1234567890)")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(CreateCustomerDto), nameof(ValidateDateOfBirth))]
    public DateTime DateOfBirth { get; set; }

    public static ValidationResult? ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;

        if (dateOfBirth > today)
        {
            return new ValidationResult("Date of birth cannot be in the future");
        }

        if (age < 18)
        {
            return new ValidationResult("Customer must be at least 18 years old");
        }

        if (age > 120)
        {
            return new ValidationResult("Date of birth seems unrealistic (age over 120 years)");
        }

        return ValidationResult.Success;
    }
}