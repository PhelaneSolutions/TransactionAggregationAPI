using FluentAssertions;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Transaction_Should_Initialize_With_Valid_Properties()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var transactionDate = DateTime.Now;

        // Act
        var transaction = new Transaction
        {
            Id = transactionId,
            AccountId = accountId.ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 250.50m,
            Description = "Coffee Shop Purchase",
            TransactionDate = transactionDate,
            Type = TransactionType.Debit,
            Category = TransactionCategory.Food,
            MerchantName = "Starbucks",
            Currency = "USD",
            Status = TransactionStatus.Completed,
            DataSource = "BankA",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Id.Should().Be(transactionId);
        transaction.AccountId.Should().Be(accountId.ToString());
        transaction.Amount.Should().Be(250.50m);
        transaction.Description.Should().Be("Coffee Shop Purchase");
        transaction.TransactionDate.Should().Be(transactionDate);
        transaction.Type.Should().Be(TransactionType.Debit);
        transaction.Category.Should().Be(TransactionCategory.Food);
        transaction.MerchantName.Should().Be("Starbucks");
        transaction.Currency.Should().Be("USD");
        transaction.Status.Should().Be(TransactionStatus.Completed);
        transaction.DataSource.Should().Be("BankA");
    }

    [Theory]
    [InlineData(TransactionType.Credit)]
    [InlineData(TransactionType.Debit)]
    [InlineData(TransactionType.Transfer)]
    [InlineData(TransactionType.Fee)]
    [InlineData(TransactionType.Interest)]
    [InlineData(TransactionType.Dividend)]
    [InlineData(TransactionType.Refund)]
    public void Transaction_Should_Support_All_Transaction_Types(TransactionType transactionType)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 100.00m,
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = transactionType,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Assert
        transaction.Type.Should().Be(transactionType);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(1000.00)]
    [InlineData(99999.99)]
    public void Transaction_Should_Handle_Positive_Amounts(decimal amount)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = amount,
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Credit,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Assert
        transaction.Amount.Should().Be(amount);
        transaction.Amount.Should().BePositive();
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1000.00)]
    [InlineData(-99999.99)]
    public void Transaction_Should_Handle_Negative_Amounts(decimal amount)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = amount,
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Debit,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Assert
        transaction.Amount.Should().Be(amount);
        transaction.Amount.Should().BeNegative();
    }

    [Fact]
    public void Transaction_Should_Handle_Zero_Amount()
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 0m,
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Fee,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Assert
        transaction.Amount.Should().Be(0m);
    }

    [Fact]
    public void Transaction_Should_Handle_Future_Dates()
    {
        // Arrange
        var futureDate = DateTime.Now.AddDays(30);

        // Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 100.00m,
            Description = "Scheduled Payment",
            TransactionDate = futureDate,
            Type = TransactionType.Transfer,
            Currency = "USD",
            Status = TransactionStatus.Pending
        };

        // Assert
        transaction.TransactionDate.Should().Be(futureDate);
        transaction.TransactionDate.Should().BeAfter(DateTime.Now);
    }
}