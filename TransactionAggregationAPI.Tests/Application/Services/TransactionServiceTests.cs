using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Services;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Tests.Application.Services;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ITransactionCategorizationService> _mockCategorizationService;
    private readonly Mock<IDataSourceService> _mockDataSourceService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TransactionService>> _mockLogger;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _mockCategorizationService = new Mock<ITransactionCategorizationService>();
        _mockDataSourceService = new Mock<IDataSourceService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TransactionService>>();
        _transactionService = new TransactionService(
            _mockTransactionRepository.Object,
            _mockCategorizationService.Object,
            new[] { _mockDataSourceService.Object },
            _mockMapper.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetTransactionsByAccountIdAsync_Should_Return_Transactions()
    {
        // Arrange
        var accountId = "1";
        var transactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), AccountId = accountId, Amount = 100.00m, Description = "Test 1" },
            new Transaction { Id = Guid.NewGuid(), AccountId = accountId, Amount = 200.00m, Description = "Test 2" }
        };

        var transactionDtos = new List<TransactionDto>
        {
            new TransactionDto { Id = transactions[0].Id, AccountId = accountId, Amount = 100.00m, Description = "Test 1" },
            new TransactionDto { Id = transactions[1].Id, AccountId = accountId, Amount = 200.00m, Description = "Test 2" }
        };

        _mockTransactionRepository.Setup(x => x.GetByAccountIdAsync(accountId)).ReturnsAsync(transactions);
        _mockMapper.Setup(x => x.Map<IEnumerable<TransactionDto>>(transactions)).Returns(transactionDtos);

        // Act
        var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(transactionDtos);
        _mockTransactionRepository.Verify(x => x.GetByAccountIdAsync(accountId), Times.Once);
    }

    [Fact]
    public async Task GetTransactionsByDateRangeAsync_Should_Return_Filtered_Transactions()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(-30);
        var endDate = DateTime.Now;
        var transactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), TransactionDate = DateTime.Now.AddDays(-15), Amount = 100.00m },
            new Transaction { Id = Guid.NewGuid(), TransactionDate = DateTime.Now.AddDays(-10), Amount = 200.00m }
        };

        var transactionDtos = new List<TransactionDto>
        {
            new TransactionDto { Id = transactions[0].Id, TransactionDate = transactions[0].TransactionDate, Amount = 100.00m },
            new TransactionDto { Id = transactions[1].Id, TransactionDate = transactions[1].TransactionDate, Amount = 200.00m }
        };

        _mockTransactionRepository.Setup(x => x.GetByDateRangeAsync(startDate, endDate)).ReturnsAsync(transactions);
        _mockMapper.Setup(x => x.Map<IEnumerable<TransactionDto>>(transactions)).Returns(transactionDtos);

        // Act
        var result = await _transactionService.GetTransactionsByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(transactionDtos);
        _mockTransactionRepository.Verify(x => x.GetByDateRangeAsync(startDate, endDate), Times.Once);
    }

    [Fact]
    public async Task GetTransactionsByCategoryAsync_Should_Return_Categorized_Transactions()
    {
        // Arrange
        var category = TransactionCategory.Food;
        var transactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), Category = category, Amount = 50.00m, Description = "Restaurant" },
            new Transaction { Id = Guid.NewGuid(), Category = category, Amount = 25.00m, Description = "Coffee" }
        };

        var transactionDtos = new List<TransactionDto>
        {
            new TransactionDto { Id = transactions[0].Id, Category = category, Amount = 50.00m, Description = "Restaurant" },
            new TransactionDto { Id = transactions[1].Id, Category = category, Amount = 25.00m, Description = "Coffee" }
        };

        _mockTransactionRepository.Setup(x => x.GetByCategoryAsync(category)).ReturnsAsync(transactions);
        _mockMapper.Setup(x => x.Map<IEnumerable<TransactionDto>>(transactions)).Returns(transactionDtos);

        // Act
        var result = await _transactionService.GetTransactionsByCategoryAsync(category);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(transactionDtos);
        _mockTransactionRepository.Verify(x => x.GetByCategoryAsync(category), Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_Should_Create_Transaction_With_Categorization()
    {
        // Arrange
        var createTransactionDto = new CreateTransactionDto
        {
            CustomerId = "1",
            AccountId = "1",
            Amount = 100.00m,
            Description = "Starbucks Coffee",
            TransactionDate = DateTime.Now,
            Type = TransactionType.Debit
        };

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            CustomerId = createTransactionDto.CustomerId,
            AccountId = createTransactionDto.AccountId,
            Amount = createTransactionDto.Amount,
            Description = createTransactionDto.Description,
            TransactionDate = createTransactionDto.TransactionDate,
            Type = createTransactionDto.Type
        };

        var categorizedTransaction = new Transaction
        {
            Id = transaction.Id,
            CustomerId = transaction.CustomerId,
            AccountId = transaction.AccountId,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            Type = transaction.Type,
            Category = TransactionCategory.Food
        };

        var createdTransactionDto = new TransactionDto
        {
            Id = categorizedTransaction.Id,
            CustomerId = categorizedTransaction.CustomerId,
            AccountId = categorizedTransaction.AccountId,
            Amount = categorizedTransaction.Amount,
            Description = categorizedTransaction.Description,
            TransactionDate = categorizedTransaction.TransactionDate,
            Type = categorizedTransaction.Type,
            Category = categorizedTransaction.Category
        };

        _mockMapper.Setup(x => x.Map<Transaction>(createTransactionDto)).Returns(transaction);
        _mockCategorizationService.Setup(x => x.CategorizeTransactionAsync(transaction)).ReturnsAsync(TransactionCategory.Food);
        _mockTransactionRepository.Setup(x => x.CreateAsync(It.IsAny<Transaction>())).ReturnsAsync(categorizedTransaction);
        _mockMapper.Setup(x => x.Map<TransactionDto>(categorizedTransaction)).Returns(createdTransactionDto);

        // Act
        var result = await _transactionService.CreateTransactionAsync(createTransactionDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdTransactionDto);
        result.Category.Should().Be(TransactionCategory.Food);
        _mockCategorizationService.Verify(x => x.CategorizeTransactionAsync(It.IsAny<Transaction>()), Times.Once);
        _mockTransactionRepository.Verify(x => x.CreateAsync(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public async Task GetTransactionsByDateRangeAsync_Should_Handle_Invalid_Date_Range()
    {
        // Arrange
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(-30); // End before start

        _mockTransactionRepository.Setup(x => x.GetByDateRangeAsync(startDate, endDate)).ReturnsAsync(new List<Transaction>());
        _mockMapper.Setup(x => x.Map<IEnumerable<TransactionDto>>(It.IsAny<List<Transaction>>())).Returns(new List<TransactionDto>());

        // Act
        var result = await _transactionService.GetTransactionsByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AggregateTransactionsFromDataSourcesAsync_Should_Process_External_Data()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = "cust1", FirstName = "John", LastName = "Doe" }
        };

        var dataSourceTransactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), Amount = 100.00m, Description = "External Transaction", DataSource = "BankA", CustomerId = "cust1", AccountId = "acc1" }
        };

        _mockDataSourceService.Setup(x => x.CheckHealthAsync()).ReturnsAsync(true);
        _mockDataSourceService.Setup(x => x.GetCustomersAsync()).ReturnsAsync(customers);
        _mockDataSourceService.Setup(x => x.GetTransactionsAsync(It.IsAny<string>(), null, null)).ReturnsAsync(dataSourceTransactions);
        _mockCategorizationService.Setup(x => x.CategorizeTransactionsAsync(It.IsAny<IEnumerable<Transaction>>())).ReturnsAsync(dataSourceTransactions);
        _mockTransactionRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Transaction?)null);
        _mockTransactionRepository.Setup(x => x.CreateAsync(It.IsAny<Transaction>())).ReturnsAsync(It.IsAny<Transaction>());

        // Act
        await _transactionService.AggregateTransactionsFromDataSourcesAsync();

        // Assert
        _mockDataSourceService.Verify(x => x.CheckHealthAsync(), Times.Once);
        _mockDataSourceService.Verify(x => x.GetCustomersAsync(), Times.Once);
        _mockDataSourceService.Verify(x => x.GetTransactionsAsync(It.IsAny<string>(), null, null), Times.Once);
        _mockCategorizationService.Verify(x => x.CategorizeTransactionsAsync(It.IsAny<IEnumerable<Transaction>>()), Times.Once);
        _mockTransactionRepository.Verify(x => x.CreateAsync(It.IsAny<Transaction>()), Times.Once);
    }
}