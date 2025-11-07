using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Services;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Tests.Application.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<AccountService>> _mockLogger;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<AccountService>>();
        _accountService = new AccountService(_mockAccountRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAccountsAsync_Should_Return_All_Accounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account { Id = "acc1", AccountNumber = "123456789", Type = AccountType.Checking, Balance = 1000.00m },
            new Account { Id = "acc2", AccountNumber = "987654321", Type = AccountType.Savings, Balance = 5000.00m }
        };

        var accountDtos = new List<AccountDto>
        {
            new AccountDto { Id = "acc1", AccountNumber = "123456789", Type = AccountType.Checking, Balance = 1000.00m },
            new AccountDto { Id = "acc2", AccountNumber = "987654321", Type = AccountType.Savings, Balance = 5000.00m }
        };

        _mockAccountRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(accounts);
        _mockMapper.Setup(x => x.Map<IEnumerable<AccountDto>>(accounts)).Returns(accountDtos);

        // Act
        var result = await _accountService.GetAllAccountsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(accountDtos);
        _mockAccountRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAccountByIdAsync_Should_Return_Account_When_Found()
    {
        // Arrange
        var accountId = "acc1";
        var account = new Account 
        { 
            Id = accountId, 
            AccountNumber = "123456789", 
            Type = AccountType.Checking, 
            Balance = 1000.00m,
            CustomerId = "cust1"
        };

        var accountDto = new AccountDto 
        { 
            Id = accountId, 
            AccountNumber = "123456789", 
            Type = AccountType.Checking, 
            Balance = 1000.00m,
            CustomerId = account.CustomerId
        };

        _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mockMapper.Setup(x => x.Map<AccountDto>(account)).Returns(accountDto);

        // Act
        var result = await _accountService.GetAccountByIdAsync(accountId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(accountDto);
        _mockAccountRepository.Verify(x => x.GetByIdAsync(accountId), Times.Once);
    }

    [Fact]
    public async Task GetAccountByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var accountId = "acc1";
        _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

        // Act
        var result = await _accountService.GetAccountByIdAsync(accountId);

        // Assert
        result.Should().BeNull();
        _mockAccountRepository.Verify(x => x.GetByIdAsync(accountId), Times.Once);
        _mockMapper.Verify(x => x.Map<AccountDto>(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public async Task GetAccountsByCustomerIdAsync_Should_Return_Customer_Accounts()
    {
        // Arrange
        var customerId = "cust1";
        var accounts = new List<Account>
        {
            new Account { Id = "acc1", CustomerId = customerId, AccountNumber = "123456789", Type = AccountType.Checking, Balance = 1000.00m },
            new Account { Id = "acc2", CustomerId = customerId, AccountNumber = "987654321", Type = AccountType.Savings, Balance = 5000.00m }
        };

        var accountDtos = new List<AccountDto>
        {
            new AccountDto { Id = "acc1", CustomerId = customerId, AccountNumber = "123456789", Type = AccountType.Checking, Balance = 1000.00m },
            new AccountDto { Id = "acc2", CustomerId = customerId, AccountNumber = "987654321", Type = AccountType.Savings, Balance = 5000.00m }
        };

        _mockAccountRepository.Setup(x => x.GetByCustomerIdAsync(customerId)).ReturnsAsync(accounts);
        _mockMapper.Setup(x => x.Map<IEnumerable<AccountDto>>(accounts)).Returns(accountDtos);

        // Act
        var result = await _accountService.GetAccountsByCustomerIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(accountDtos);
        result.Should().OnlyContain(a => a.CustomerId == customerId);
        _mockAccountRepository.Verify(x => x.GetByCustomerIdAsync(customerId), Times.Once);
    }

    [Fact]
    public async Task CreateAccountAsync_Should_Create_And_Return_Account()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            CustomerId = "cust1",
            AccountName = "Test Account",
            Type = AccountType.Checking,
            Currency = "USD",
            InitialBalance = 1000.00m
        };

        var account = new Account
        {
            Id = "acc1",
            CustomerId = createAccountDto.CustomerId,
            AccountNumber = "123456789",
            AccountName = createAccountDto.AccountName,
            Type = createAccountDto.Type,
            Currency = createAccountDto.Currency,
            Balance = createAccountDto.InitialBalance
        };

        var createdAccountDto = new AccountDto
        {
            Id = account.Id,
            CustomerId = account.CustomerId,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            Type = account.Type,
            Currency = account.Currency,
            Balance = account.Balance
        };

        _mockMapper.Setup(x => x.Map<Account>(createAccountDto)).Returns(account);
        _mockAccountRepository.Setup(x => x.CreateAsync(account)).ReturnsAsync(account);
        _mockMapper.Setup(x => x.Map<AccountDto>(account)).Returns(createdAccountDto);

        // Act
        var result = await _accountService.CreateAccountAsync(createAccountDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdAccountDto);
        result.Id.Should().NotBeEmpty();
        _mockMapper.Verify(x => x.Map<Account>(createAccountDto), Times.Once);
        _mockAccountRepository.Verify(x => x.CreateAsync(It.IsAny<Account>()), Times.Once);
        _mockMapper.Verify(x => x.Map<AccountDto>(account), Times.Once);
    }

    [Theory]
    [InlineData(AccountType.Checking)]
    [InlineData(AccountType.Savings)]
    [InlineData(AccountType.Credit)]
    [InlineData(AccountType.Investment)]
    [InlineData(AccountType.Business)]
    [InlineData(AccountType.Loan)]
    public async Task CreateAccountAsync_Should_Support_All_Account_Types(AccountType accountType)
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            CustomerId = "cust1",
            AccountName = "Test Account",
            Type = accountType,
            Currency = "USD",
            InitialBalance = 1000.00m
        };

        var account = new Account
        {
            Id = "acc1",
            CustomerId = createAccountDto.CustomerId,
            AccountNumber = "123456789",
            AccountName = createAccountDto.AccountName,
            Type = accountType,
            Currency = createAccountDto.Currency,
            Balance = createAccountDto.InitialBalance
        };

        var createdAccountDto = new AccountDto
        {
            Id = account.Id,
            CustomerId = account.CustomerId,
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            Type = accountType,
            Currency = account.Currency,
            Balance = account.Balance
        };

        _mockMapper.Setup(x => x.Map<Account>(createAccountDto)).Returns(account);
        _mockAccountRepository.Setup(x => x.CreateAsync(account)).ReturnsAsync(account);
        _mockMapper.Setup(x => x.Map<AccountDto>(account)).Returns(createdAccountDto);

        // Act
        var result = await _accountService.CreateAccountAsync(createAccountDto);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(accountType);
        _mockAccountRepository.Verify(x => x.CreateAsync(It.IsAny<Account>()), Times.Once);
    }
}