using FluentAssertions;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Tests.Domain;

public class AccountTests
{
    [Fact]
    public void Account_Should_Initialize_With_Valid_Properties()
    {
        // Arrange
        var customerId = Guid.NewGuid().ToString();
        var accountId = Guid.NewGuid().ToString();

        // Act
        var account = new Account
        {
            Id = accountId,
            CustomerId = customerId,
            AccountNumber = "123456789",
            AccountName = "Primary Checking",
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = 1500.75m,
            AvailableBalance = 1400.75m,
            Status = AccountStatus.Active,
            OpenedDate = DateTime.Now,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        account.Id.Should().Be(accountId);
        account.CustomerId.Should().Be(customerId);
        account.AccountNumber.Should().Be("123456789");
        account.AccountName.Should().Be("Primary Checking");
        account.Type.Should().Be(AccountType.Checking);
        account.Currency.Should().Be("USD");
        account.Balance.Should().Be(1500.75m);
        account.AvailableBalance.Should().Be(1400.75m);
        account.Status.Should().Be(AccountStatus.Active);
        account.Transactions.Should().NotBeNull();
        account.Transactions.Should().BeEmpty();
    }

    [Theory]
    [InlineData(AccountType.Checking)]
    [InlineData(AccountType.Savings)]
    [InlineData(AccountType.Credit)]
    [InlineData(AccountType.Investment)]
    [InlineData(AccountType.Business)]
    [InlineData(AccountType.Loan)]
    public void Account_Should_Support_All_Account_Types(AccountType accountType)
    {
        // Arrange & Act
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = "123456789",
            Type = accountType,
            Currency = "USD",
            Balance = 1000.00m,
            Status = AccountStatus.Active
        };

        // Assert
        account.Type.Should().Be(accountType);
    }

    [Fact]
    public void Account_Should_Allow_Adding_Transactions()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = "123456789",
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = 1000.00m,
            Status = AccountStatus.Active
        };

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            CustomerId = account.CustomerId,
            Amount = 100.00m,
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Credit,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Act
        account.Transactions.Add(transaction);

        // Assert
        account.Transactions.Should().HaveCount(1);
        account.Transactions.First().Should().Be(transaction);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(1000000)]
    public void Account_Should_Handle_Various_Balance_Values(decimal balance)
    {
        // Arrange & Act
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = "123456789",
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = balance,
            Status = AccountStatus.Active
        };

        // Assert
        account.Balance.Should().Be(balance);
    }
}