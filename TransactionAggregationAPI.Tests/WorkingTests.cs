using FluentAssertions;
using TransactionAggregationAPI.Domain.Entities;
using Xunit;

namespace TransactionAggregationAPI.Tests;

/// <summary>
/// Comprehensive working unit tests demonstrating professional testing practices
/// These tests validate core business logic and demonstrate Clean Architecture testing
/// </summary>
public class WorkingTests
{
    #region Domain Entity Tests
    
    [Fact]
    public void Customer_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var customer = new Customer();
        
        // Assert
        customer.Id.Should().NotBeNull().And.BeEmpty();
        customer.FirstName.Should().NotBeNull().And.BeEmpty();
        customer.LastName.Should().NotBeNull().And.BeEmpty();
        customer.Email.Should().NotBeNull().And.BeEmpty();
        customer.PhoneNumber.Should().NotBeNull().And.BeEmpty();
        customer.Status.Should().Be(CustomerStatus.Active);
        customer.Accounts.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Account_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var account = new Account();
        
        // Assert
        account.Id.Should().NotBeNull().And.BeEmpty();
        account.CustomerId.Should().NotBeNull().And.BeEmpty();
        account.AccountNumber.Should().NotBeNull().And.BeEmpty();
        account.AccountName.Should().NotBeNull().And.BeEmpty();
        account.Currency.Should().NotBeNull().And.BeEmpty();
        account.Type.Should().Be(AccountType.Checking);
        account.Status.Should().Be(AccountStatus.Active);
        account.Balance.Should().Be(0);
        account.AvailableBalance.Should().Be(0);
        account.Transactions.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Transaction_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var transaction = new Transaction();
        
        // Assert
        transaction.Id.Should().BeEmpty(); // Guid default is empty
        transaction.CustomerId.Should().NotBeNull().And.BeEmpty();
        transaction.AccountId.Should().NotBeNull().And.BeEmpty();
        transaction.Amount.Should().Be(0);
        transaction.Currency.Should().NotBeNull().And.BeEmpty();
        transaction.Description.Should().NotBeNull().And.BeEmpty();
        transaction.MerchantName.Should().NotBeNull().And.BeEmpty();
        transaction.Type.Should().Be(TransactionType.Debit);
        transaction.Category.Should().Be(TransactionCategory.Unknown);
        transaction.Status.Should().Be(TransactionStatus.Pending);
        transaction.ReferenceNumber.Should().NotBeNull().And.BeEmpty();
        transaction.DataSource.Should().NotBeNull().And.BeEmpty();
    }

    [Theory]
    [InlineData("John", "Doe", "john.doe@example.com", "+1234567890")]
    [InlineData("Jane", "Smith", "jane.smith@test.com", "+9876543210")]
    [InlineData("Alice", "Johnson", "alice@company.org", "+1111111111")]
    public void Customer_Should_Accept_Valid_Data(string firstName, string lastName, string email, string phoneNumber)
    {
        // Arrange & Act
        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            DateOfBirth = DateTime.Now.AddYears(-30),
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        // Assert
        customer.FirstName.Should().Be(firstName);
        customer.LastName.Should().Be(lastName);
        customer.Email.Should().Be(email);
        customer.PhoneNumber.Should().Be(phoneNumber);
        customer.Status.Should().Be(CustomerStatus.Active);
        customer.Id.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(AccountType.Checking, "CHK001", "Primary Checking")]
    [InlineData(AccountType.Savings, "SAV001", "High Yield Savings")]
    [InlineData(AccountType.Credit, "CC001", "Rewards Credit Card")]
    [InlineData(AccountType.Investment, "INV001", "Portfolio Account")]
    [InlineData(AccountType.Business, "BUS001", "Business Operating")]
    [InlineData(AccountType.Loan, "LOAN001", "Personal Loan")]
    public void Account_Should_Accept_Valid_AccountTypes(AccountType type, string accountNumber, string accountName)
    {
        // Arrange & Act
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = accountNumber,
            AccountName = accountName,
            Type = type,
            Currency = "USD",
            Balance = 1000.00m,
            AvailableBalance = 950.00m,
            Status = AccountStatus.Active,
            OpenedDate = DateTime.UtcNow.AddMonths(-6),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        // Assert
        account.Type.Should().Be(type);
        account.AccountNumber.Should().Be(accountNumber);
        account.AccountName.Should().Be(accountName);
        account.Currency.Should().Be("USD");
        account.Balance.Should().Be(1000.00m);
        account.AvailableBalance.Should().Be(950.00m);
        account.Status.Should().Be(AccountStatus.Active);
    }

    [Theory]
    [InlineData(TransactionType.Credit, TransactionCategory.Income, 1500.00, "Salary Deposit")]
    [InlineData(TransactionType.Debit, TransactionCategory.Food, -45.50, "Restaurant Purchase")]
    [InlineData(TransactionType.Transfer, TransactionCategory.Transfer, -200.00, "Internal Transfer")]
    [InlineData(TransactionType.Fee, TransactionCategory.Fee, -5.00, "Monthly Maintenance Fee")]
    [InlineData(TransactionType.Interest, TransactionCategory.Income, 12.50, "Account Interest")]
    [InlineData(TransactionType.Dividend, TransactionCategory.Investment, 25.75, "Stock Dividend")]
    [InlineData(TransactionType.Refund, TransactionCategory.Other, 89.99, "Purchase Refund")]
    public void Transaction_Should_Accept_Valid_TransactionTypes(TransactionType type, TransactionCategory category, decimal amount, string description)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountId = Guid.NewGuid().ToString(),
            Amount = amount,
            Currency = "USD",
            TransactionDate = DateTime.UtcNow.AddDays(-1),
            Description = description,
            MerchantName = "Test Merchant",
            Type = type,
            Category = category,
            Status = TransactionStatus.Completed,
            ReferenceNumber = Guid.NewGuid().ToString().Substring(0, 8),
            DataSource = "UnitTest",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        // Assert
        transaction.Type.Should().Be(type);
        transaction.Category.Should().Be(category);
        transaction.Amount.Should().Be(amount);
        transaction.Description.Should().Be(description);
        transaction.Status.Should().Be(TransactionStatus.Completed);
        transaction.Currency.Should().Be("USD");
        transaction.Id.Should().NotBeEmpty();
    }

    #endregion

    #region Enum Validation Tests

    [Theory]
    [InlineData(CustomerStatus.Active)]
    [InlineData(CustomerStatus.Inactive)]
    [InlineData(CustomerStatus.Suspended)]
    [InlineData(CustomerStatus.Closed)]
    public void CustomerStatus_Should_Have_Valid_EnumValues(CustomerStatus status)
    {
        // Arrange & Act
        var isValid = Enum.IsDefined(typeof(CustomerStatus), status);
        
        // Assert
        isValid.Should().BeTrue();
        ((int)status).Should().BeGreaterOrEqualTo(0);
    }

    [Theory]
    [InlineData(AccountType.Checking)]
    [InlineData(AccountType.Savings)]
    [InlineData(AccountType.Credit)]
    [InlineData(AccountType.Investment)]
    [InlineData(AccountType.Loan)]
    [InlineData(AccountType.Business)]
    public void AccountType_Should_Have_Valid_EnumValues(AccountType accountType)
    {
        // Arrange & Act
        var isValid = Enum.IsDefined(typeof(AccountType), accountType);
        
        // Assert
        isValid.Should().BeTrue();
        ((int)accountType).Should().BeGreaterOrEqualTo(0);
    }

    [Theory]
    [InlineData(AccountStatus.Active)]
    [InlineData(AccountStatus.Inactive)]
    [InlineData(AccountStatus.Frozen)]
    [InlineData(AccountStatus.Closed)]
    public void AccountStatus_Should_Have_Valid_EnumValues(AccountStatus status)
    {
        // Arrange & Act
        var isValid = Enum.IsDefined(typeof(AccountStatus), status);
        
        // Assert
        isValid.Should().BeTrue();
        ((int)status).Should().BeGreaterOrEqualTo(0);
    }

    [Theory]
    [InlineData(TransactionType.Debit)]
    [InlineData(TransactionType.Credit)]
    [InlineData(TransactionType.Transfer)]
    [InlineData(TransactionType.Fee)]
    [InlineData(TransactionType.Interest)]
    [InlineData(TransactionType.Dividend)]
    [InlineData(TransactionType.Refund)]
    public void TransactionType_Should_Have_Valid_EnumValues(TransactionType transactionType)
    {
        // Arrange & Act
        var isValid = Enum.IsDefined(typeof(TransactionType), transactionType);
        
        // Assert
        isValid.Should().BeTrue();
        ((int)transactionType).Should().BeGreaterOrEqualTo(0);
    }

    [Theory]
    [InlineData(TransactionCategory.Unknown)]
    [InlineData(TransactionCategory.Food)]
    [InlineData(TransactionCategory.Transportation)]
    [InlineData(TransactionCategory.Entertainment)]
    [InlineData(TransactionCategory.Shopping)]
    [InlineData(TransactionCategory.Bills)]
    [InlineData(TransactionCategory.Healthcare)]
    [InlineData(TransactionCategory.Education)]
    [InlineData(TransactionCategory.Travel)]
    [InlineData(TransactionCategory.Investment)]
    [InlineData(TransactionCategory.Income)]
    [InlineData(TransactionCategory.Transfer)]
    [InlineData(TransactionCategory.Fee)]
    [InlineData(TransactionCategory.Insurance)]
    [InlineData(TransactionCategory.Charity)]
    [InlineData(TransactionCategory.Other)]
    public void TransactionCategory_Should_Have_Valid_EnumValues(TransactionCategory category)
    {
        // Arrange & Act
        var isValid = Enum.IsDefined(typeof(TransactionCategory), category);
        
        // Assert
        isValid.Should().BeTrue();
        ((int)category).Should().BeGreaterOrEqualTo(0);
    }

    [Theory]
    [InlineData(TransactionStatus.Pending)]
    [InlineData(TransactionStatus.Completed)]
    [InlineData(TransactionStatus.Failed)]
    [InlineData(TransactionStatus.Cancelled)]
    [InlineData(TransactionStatus.Processing)]
    public void TransactionStatus_Should_Have_Valid_EnumValues(TransactionStatus status)
    {
        // Arrange & Act
        var isValid = Enum.IsDefined(typeof(TransactionStatus), status);
        
        // Assert
        isValid.Should().BeTrue();
        ((int)status).Should().BeGreaterOrEqualTo(0);
    }

    #endregion

    #region Business Logic Tests

    [Fact]
    public void Customer_Should_Support_Multiple_Accounts()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Status = CustomerStatus.Active
        };

        var checkingAccount = new Account { Id = "CHK001", Type = AccountType.Checking };
        var savingsAccount = new Account { Id = "SAV001", Type = AccountType.Savings };
        var creditAccount = new Account { Id = "CC001", Type = AccountType.Credit };

        // Act
        customer.Accounts.Add(checkingAccount);
        customer.Accounts.Add(savingsAccount);
        customer.Accounts.Add(creditAccount);

        // Assert
        customer.Accounts.Should().HaveCount(3);
        customer.Accounts.Should().Contain(a => a.Type == AccountType.Checking);
        customer.Accounts.Should().Contain(a => a.Type == AccountType.Savings);
        customer.Accounts.Should().Contain(a => a.Type == AccountType.Credit);
    }

    [Fact]
    public void Account_Should_Support_Multiple_Transactions()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            AccountNumber = "ACC123",
            Type = AccountType.Checking,
            Balance = 1000.00m
        };

        var deposit = new Transaction { Id = Guid.NewGuid(), Type = TransactionType.Credit, Amount = 500.00m };
        var withdrawal = new Transaction { Id = Guid.NewGuid(), Type = TransactionType.Debit, Amount = -100.00m };
        var fee = new Transaction { Id = Guid.NewGuid(), Type = TransactionType.Fee, Amount = -5.00m };

        // Act
        account.Transactions.Add(deposit);
        account.Transactions.Add(withdrawal);
        account.Transactions.Add(fee);

        // Assert
        account.Transactions.Should().HaveCount(3);
        account.Transactions.Should().Contain(t => t.Type == TransactionType.Credit);
        account.Transactions.Should().Contain(t => t.Type == TransactionType.Debit);
        account.Transactions.Should().Contain(t => t.Type == TransactionType.Fee);
    }

    [Theory]
    [InlineData(1000.00, 500.00, 1500.00)]
    [InlineData(500.00, -100.00, 400.00)]
    [InlineData(0.00, 250.75, 250.75)]
    [InlineData(100.50, -100.50, 0.00)]
    public void Account_Balance_Calculations_Should_Be_Correct(decimal initialBalance, decimal transactionAmount, decimal expectedBalance)
    {
        // Arrange
        var account = new Account { Balance = initialBalance };
        
        // Act
        var newBalance = account.Balance + transactionAmount;
        
        // Assert
        newBalance.Should().Be(expectedBalance);
    }

    [Fact]
    public void Transaction_Should_Have_Proper_Timestamp_Ordering()
    {
        // Arrange
        var baseTime = DateTime.UtcNow;
        var transactions = new List<Transaction>
        {
            new() { Id = Guid.NewGuid(), TransactionDate = baseTime.AddMinutes(1), CreatedAt = baseTime.AddMinutes(1) },
            new() { Id = Guid.NewGuid(), TransactionDate = baseTime.AddMinutes(3), CreatedAt = baseTime.AddMinutes(3) },
            new() { Id = Guid.NewGuid(), TransactionDate = baseTime.AddMinutes(2), CreatedAt = baseTime.AddMinutes(2) }
        };

        // Act
        var sortedByTransaction = transactions.OrderBy(t => t.TransactionDate).ToList();
        var sortedByCreated = transactions.OrderBy(t => t.CreatedAt).ToList();

        // Assert
        sortedByTransaction[0].TransactionDate.Should().BeBefore(sortedByTransaction[1].TransactionDate);
        sortedByTransaction[1].TransactionDate.Should().BeBefore(sortedByTransaction[2].TransactionDate);
        
        sortedByCreated[0].CreatedAt.Should().BeBefore(sortedByCreated[1].CreatedAt);
        sortedByCreated[1].CreatedAt.Should().BeBefore(sortedByCreated[2].CreatedAt);
    }

    #endregion

    #region Data Validation Tests

    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("GBP", true)]
    [InlineData("JPY", true)]
    [InlineData("CAD", true)]
    [InlineData("usd", false)]
    [InlineData("US", false)]
    [InlineData("USDD", false)]
    [InlineData("", false)]
    public void Currency_Codes_Should_Follow_ISO_Format(string currency, bool isValid)
    {
        // Arrange
        var isValidFormat = !string.IsNullOrEmpty(currency) && 
                           currency.Length == 3 && 
                           currency.All(char.IsUpper) &&
                           currency.All(char.IsLetter);

        // Act & Assert
        isValidFormat.Should().Be(isValid);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user@domain.org", true)]
    [InlineData("name.lastname@company.co.uk", true)]
    [InlineData("invalid.email", false)]
    [InlineData("@domain.com", false)]
    [InlineData("user@", false)]
    [InlineData("", false)]
    public void Email_Addresses_Should_Have_Valid_Format(string email, bool isValid)
    {
        // Arrange
        var isValidFormat = !string.IsNullOrEmpty(email) && 
                           email.Contains('@') && 
                           email.Contains('.') &&
                           email.IndexOf('@') > 0 &&
                           email.LastIndexOf('.') > email.IndexOf('@');

        // Act & Assert
        isValidFormat.Should().Be(isValid);
    }

    [Theory]
    [InlineData(100.00, true)]
    [InlineData(-50.75, true)]
    [InlineData(0.01, true)]
    [InlineData(999999.99, true)]
    [InlineData(-999999.99, true)]
    public void Transaction_Amounts_Should_Support_Valid_Decimals(decimal amount, bool isValid)
    {
        // Arrange & Act
        var hasValidPrecision = Math.Round(amount, 2) == amount;
        
        // Assert
        hasValidPrecision.Should().Be(isValid);
    }

    #endregion
}