using FluentAssertions;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Infrastructure.Repositories.Mock;

namespace TransactionAggregationAPI.Tests.Infrastructure.Repositories;

public class MockTransactionRepositoryTests
{
    private readonly MockTransactionRepository _repository;

    public MockTransactionRepositoryTests()
    {
        _repository = new MockTransactionRepository();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Transactions()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().HaveCountGreaterThan(100); // We have 120+ mock transactions
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Transaction_When_Found()
    {
        // Arrange
        var allTransactions = await _repository.GetAllAsync();
        var existingTransaction = allTransactions.First();

        // Act
        var result = await _repository.GetByIdAsync(existingTransaction.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(existingTransaction.Id);
        result.Amount.Should().Be(existingTransaction.Amount);
        result.Description.Should().Be(existingTransaction.Description);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByAccountIdAsync_Should_Return_Account_Transactions()
    {
        // Arrange
        var allTransactions = await _repository.GetAllAsync();
        var accountId = allTransactions.First().AccountId;

        // Act
        var result = await _repository.GetByAccountIdAsync(accountId);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().OnlyContain(t => t.AccountId == accountId);
    }

    [Fact]
    public async Task GetByAccountIdAsync_Should_Return_Empty_For_NonExistent_Account()
    {
        // Arrange
        var nonExistentAccountId = "nonexistent";

        // Act
        var result = await _repository.GetByAccountIdAsync(nonExistentAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByDateRangeAsync_Should_Return_Filtered_Transactions()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now.AddDays(-1);

        // Act
        var result = await _repository.GetByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().NotBeNull();
        result.Should().OnlyContain(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);
    }

    [Fact]
    public async Task GetByDateRangeAsync_Should_Return_Empty_For_Future_Dates()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(30);
        var endDate = DateTime.Now.AddDays(60);

        // Act
        var result = await _repository.GetByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByCategoryAsync_Should_Return_Categorized_Transactions()
    {
        // Arrange
        var category = TransactionCategory.Food;

        // Act
        var result = await _repository.GetByCategoryAsync(category);

        // Assert
        result.Should().NotBeNull();
        result.Should().OnlyContain(t => t.Category == category);
        if (result.Any())
        {
            result.Should().NotBeEmpty();
        }
    }

    [Fact]
    public async Task GetByCategoryAsync_Should_Return_Empty_For_NonExistent_Category()
    {
        // Arrange
        var nonExistentCategory = TransactionCategory.Unknown;

        // Act
        var result = await _repository.GetByCategoryAsync(nonExistentCategory);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateAsync_Should_Add_New_Transaction()
    {
        // Arrange
        var newTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = "acc1",
            CustomerId = "cust1",
            Amount = 999.99m,
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Debit,
            Category = TransactionCategory.Other
        };

        var initialCount = (await _repository.GetAllAsync()).Count();

        // Act
        var result = await _repository.CreateAsync(newTransaction);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(newTransaction.Id);
        result.Amount.Should().Be(999.99m);
        result.Description.Should().Be("Test Transaction");

        var allTransactionsAfter = await _repository.GetAllAsync();
        allTransactionsAfter.Should().HaveCount(initialCount + 1);

        var createdTransaction = await _repository.GetByIdAsync(newTransaction.Id);
        createdTransaction.Should().NotBeNull();
        createdTransaction!.Should().BeEquivalentTo(newTransaction);
    }

    [Fact]
    public async Task UpdateAsync_Should_Modify_Existing_Transaction()
    {
        // Arrange
        var allTransactions = await _repository.GetAllAsync();
        var existingTransaction = allTransactions.First();
        var originalDescription = existingTransaction.Description;

        existingTransaction.Description = "Updated Test Description";
        existingTransaction.Amount = 1234.56m;

        // Act
        var result = await _repository.UpdateAsync(existingTransaction);

        // Assert
        result.Should().NotBeNull();
        result.Description.Should().Be("Updated Test Description");
        result.Amount.Should().Be(1234.56m);
        result.Description.Should().NotBe(originalDescription);

        var updatedTransaction = await _repository.GetByIdAsync(existingTransaction.Id);
        updatedTransaction.Should().NotBeNull();
        updatedTransaction!.Description.Should().Be("Updated Test Description");
        updatedTransaction.Amount.Should().Be(1234.56m);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_For_NonExistent_Transaction()
    {
        // Arrange
        var nonExistentTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = "acc1",
            CustomerId = "cust1",
            Amount = 100.00m,
            Description = "Non-existent",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Debit
        };

        // Act
        var result = await _repository.UpdateAsync(nonExistentTransaction);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Existing_Transaction()
    {
        // Arrange
        var allTransactions = await _repository.GetAllAsync();
        var transactionToDelete = allTransactions.First();
        var initialCount = allTransactions.Count();

        // Act
        await _repository.DeleteAsync(transactionToDelete.Id);

        // Assert
        var allTransactionsAfter = await _repository.GetAllAsync();
        allTransactionsAfter.Should().HaveCount(initialCount - 1);

        var deletedTransaction = await _repository.GetByIdAsync(transactionToDelete.Id);
        deletedTransaction.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_For_NonExistent_Transaction()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var initialCount = (await _repository.GetAllAsync()).Count();

        // Act & Assert
        var deleteAction = () => _repository.DeleteAsync(nonExistentId);
        await deleteAction.Should().NotThrowAsync();

        var countAfter = (await _repository.GetAllAsync()).Count();
        countAfter.Should().Be(initialCount);
    }

    [Fact]
    public async Task Repository_Should_Maintain_Data_Consistency()
    {
        // Arrange
        var initialTransactions = await _repository.GetAllAsync();
        var initialCount = initialTransactions.Count();

        var newTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = initialTransactions.First().AccountId, // Use existing account
            CustomerId = initialTransactions.First().CustomerId,
            Amount = 100.00m,
            Description = "Consistency Test",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Credit,
            Category = TransactionCategory.Other
        };

        // Act - Create, Update, and verify consistency
        await _repository.CreateAsync(newTransaction);
        var createdTransaction = await _repository.GetByIdAsync(newTransaction.Id);
        
        createdTransaction!.Amount = 200.00m;
        await _repository.UpdateAsync(createdTransaction);
        
        var updatedTransaction = await _repository.GetByIdAsync(newTransaction.Id);

        // Assert
        updatedTransaction.Should().NotBeNull();
        updatedTransaction!.Amount.Should().Be(200.00m);
        updatedTransaction.Description.Should().Be("Consistency Test");

        var finalCount = (await _repository.GetAllAsync()).Count();
        finalCount.Should().Be(initialCount + 1);
    }

    [Theory]
    [InlineData(TransactionType.Credit)]
    [InlineData(TransactionType.Debit)]
    [InlineData(TransactionType.Transfer)]
    [InlineData(TransactionType.Fee)]
    public async Task Repository_Should_Handle_All_Transaction_Types(TransactionType transactionType)
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 100.00m,
            Description = $"Test {transactionType}",
            TransactionDate = DateTime.Now,
            Type = transactionType,
            Category = TransactionCategory.Other,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Act
        var result = await _repository.CreateAsync(transaction);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(transactionType);

        var retrievedTransaction = await _repository.GetByIdAsync(transaction.Id);
        retrievedTransaction.Should().NotBeNull();
        retrievedTransaction!.Type.Should().Be(transactionType);
    }
}