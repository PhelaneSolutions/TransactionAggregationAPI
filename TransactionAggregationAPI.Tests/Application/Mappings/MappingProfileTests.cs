using AutoMapper;
using FluentAssertions;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Mappings;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Tests.Application.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Should_Be_Valid()
    {
        // Act & Assert - This will throw if mapping configuration is invalid
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_Customer_To_CustomerDto()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "555-1234",
            DateOfBirth = new DateTime(1990, 1, 1),
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<CustomerDto>(customer);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customer.Id);
        result.FirstName.Should().Be(customer.FirstName);
        result.LastName.Should().Be(customer.LastName);
        result.Email.Should().Be(customer.Email);
        result.PhoneNumber.Should().Be(customer.PhoneNumber);
        result.DateOfBirth.Should().Be(customer.DateOfBirth);
        result.Status.Should().Be(customer.Status);
    }

    [Fact]
    public void Should_Map_CustomerDto_To_Customer()
    {
        // Arrange
        var customerDto = new CustomerDto
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "555-5678",
            DateOfBirth = new DateTime(1985, 5, 15),
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<Customer>(customerDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customerDto.Id);
        result.FirstName.Should().Be(customerDto.FirstName);
        result.LastName.Should().Be(customerDto.LastName);
        result.Email.Should().Be(customerDto.Email);
        result.PhoneNumber.Should().Be(customerDto.PhoneNumber);
        result.DateOfBirth.Should().Be(customerDto.DateOfBirth);
        result.Status.Should().Be(customerDto.Status);
    }

    [Fact]
    public void Should_Map_Account_To_AccountDto()
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = "123456789",
            AccountName = "Checking Account",
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = 1500.75m,
            AvailableBalance = 1500.75m,
            Status = AccountStatus.Active,
            OpenedDate = DateTime.Now,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<AccountDto>(account);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(account.Id);
        result.CustomerId.Should().Be(account.CustomerId);
        result.AccountNumber.Should().Be(account.AccountNumber);
        result.AccountName.Should().Be(account.AccountName);
        result.Type.Should().Be(account.Type);
        result.Currency.Should().Be(account.Currency);
        result.Balance.Should().Be(account.Balance);
        result.AvailableBalance.Should().Be(account.AvailableBalance);
        result.Status.Should().Be(account.Status);
        result.OpenedDate.Should().Be(account.OpenedDate);
    }

    [Fact]
    public void Should_Map_AccountDto_To_Account()
    {
        // Arrange
        var accountDto = new AccountDto
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = "987654321",
            AccountName = "Savings Account",
            Type = AccountType.Savings,
            Currency = "USD",
            Balance = 25000.50m,
            AvailableBalance = 25000.50m,
            Status = AccountStatus.Active,
            OpenedDate = DateTime.Now.AddYears(-2),
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<Account>(accountDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(accountDto.Id);
        result.CustomerId.Should().Be(accountDto.CustomerId);
        result.AccountNumber.Should().Be(accountDto.AccountNumber);
        result.AccountName.Should().Be(accountDto.AccountName);
        result.Type.Should().Be(accountDto.Type);
        result.Currency.Should().Be(accountDto.Currency);
        result.Balance.Should().Be(accountDto.Balance);
        result.AvailableBalance.Should().Be(accountDto.AvailableBalance);
        result.Status.Should().Be(accountDto.Status);
        result.OpenedDate.Should().Be(accountDto.OpenedDate);
    }

    [Fact]
    public void Should_Map_Transaction_To_TransactionDto()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 250.75m,
            Currency = "USD",
            TransactionDate = DateTime.Now,
            Description = "Coffee Shop Purchase",
            MerchantName = "Starbucks",
            Type = TransactionType.Debit,
            Category = TransactionCategory.Food,
            Status = TransactionStatus.Completed,
            ReferenceNumber = "REF123",
            DataSource = "BankA",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<TransactionDto>(transaction);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(transaction.Id);
        result.AccountId.Should().Be(transaction.AccountId);
        result.CustomerId.Should().Be(transaction.CustomerId);
        result.Amount.Should().Be(transaction.Amount);
        result.Currency.Should().Be(transaction.Currency);
        result.TransactionDate.Should().Be(transaction.TransactionDate);
        result.Description.Should().Be(transaction.Description);
        result.MerchantName.Should().Be(transaction.MerchantName);
        result.Type.Should().Be(transaction.Type);
        result.Category.Should().Be(transaction.Category);
        result.Status.Should().Be(transaction.Status);
        result.ReferenceNumber.Should().Be(transaction.ReferenceNumber);
        result.DataSource.Should().Be(transaction.DataSource);
    }

    [Fact]
    public void Should_Map_TransactionDto_To_Transaction()
    {
        // Arrange
        var transactionDto = new TransactionDto
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 1500.00m,
            Currency = "USD",
            TransactionDate = DateTime.Now,
            Description = "Salary Deposit",
            MerchantName = "Employer Corp",
            Type = TransactionType.Credit,
            Category = TransactionCategory.Income,
            Status = TransactionStatus.Completed,
            ReferenceNumber = "REF456",
            DataSource = "Payroll",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<Transaction>(transactionDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(transactionDto.Id);
        result.AccountId.Should().Be(transactionDto.AccountId);
        result.CustomerId.Should().Be(transactionDto.CustomerId);
        result.Amount.Should().Be(transactionDto.Amount);
        result.Currency.Should().Be(transactionDto.Currency);
        result.TransactionDate.Should().Be(transactionDto.TransactionDate);
        result.Description.Should().Be(transactionDto.Description);
        result.MerchantName.Should().Be(transactionDto.MerchantName);
        result.Type.Should().Be(transactionDto.Type);
        result.Category.Should().Be(transactionDto.Category);
        result.Status.Should().Be(transactionDto.Status);
        result.ReferenceNumber.Should().Be(transactionDto.ReferenceNumber);
        result.DataSource.Should().Be(transactionDto.DataSource);
    }

    [Theory]
    [InlineData(AccountType.Checking)]
    [InlineData(AccountType.Savings)]
    [InlineData(AccountType.Credit)]
    [InlineData(AccountType.Investment)]
    [InlineData(AccountType.Business)]
    [InlineData(AccountType.Loan)]
    public void Should_Map_All_Account_Types_Correctly(AccountType accountType)
    {
        // Arrange
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            AccountNumber = "123456789",
            AccountName = "Test Account",
            Type = accountType,
            Currency = "USD",
            Balance = 1000.00m,
            AvailableBalance = 1000.00m,
            Status = AccountStatus.Active,
            OpenedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var accountDto = _mapper.Map<AccountDto>(account);
        var mappedBackAccount = _mapper.Map<Account>(accountDto);

        // Assert
        accountDto.Type.Should().Be(accountType);
        mappedBackAccount.Type.Should().Be(accountType);
    }

    [Theory]
    [InlineData(TransactionType.Credit)]
    [InlineData(TransactionType.Debit)]
    [InlineData(TransactionType.Transfer)]
    [InlineData(TransactionType.Fee)]
    [InlineData(TransactionType.Interest)]
    [InlineData(TransactionType.Refund)]
    [InlineData(TransactionType.Dividend)]
    public void Should_Map_All_Transaction_Types_Correctly(TransactionType transactionType)
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid().ToString(),
            CustomerId = Guid.NewGuid().ToString(),
            Amount = 100.00m,
            Currency = "USD",
            Description = "Test Transaction",
            TransactionDate = DateTime.Now,
            Type = transactionType,
            Category = TransactionCategory.Unknown,
            Status = TransactionStatus.Completed,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var transactionDto = _mapper.Map<TransactionDto>(transaction);
        var mappedBackTransaction = _mapper.Map<Transaction>(transactionDto);

        // Assert
        transactionDto.Type.Should().Be(transactionType);
        mappedBackTransaction.Type.Should().Be(transactionType);
    }

    [Fact]
    public void Should_Handle_Null_Values_Gracefully()
    {
        // Arrange
        Customer? nullCustomer = null;
        Account? nullAccount = null;
        Transaction? nullTransaction = null;

        // Act
        var customerDto = _mapper.Map<CustomerDto>(nullCustomer);
        var accountDto = _mapper.Map<AccountDto>(nullAccount);
        var transactionDto = _mapper.Map<TransactionDto>(nullTransaction);

        // Assert
        customerDto.Should().BeNull();
        accountDto.Should().BeNull();
        transactionDto.Should().BeNull();
    }

    [Fact]
    public void Should_Map_Collections_Correctly()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid().ToString(), FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-1234", DateOfBirth = new DateTime(1990, 1, 1), Status = CustomerStatus.Active, CreatedAt = DateTime.UtcNow },
            new Customer { Id = Guid.NewGuid().ToString(), FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "555-5678", DateOfBirth = new DateTime(1985, 5, 15), Status = CustomerStatus.Active, CreatedAt = DateTime.UtcNow }
        };

        // Act
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

        // Assert
        customerDtos.Should().NotBeNull();
        customerDtos.Should().HaveCount(2);
        customerDtos.First().FirstName.Should().Be("John");
        customerDtos.Last().FirstName.Should().Be("Jane");
    }

    [Fact]
    public void Should_Update_Existing_Entity_When_Mapping()
    {
        // Arrange
        var existingCustomer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Original",
            LastName = "Name",
            Email = "original@example.com",
            PhoneNumber = "555-0000",
            DateOfBirth = new DateTime(1990, 1, 1),
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var customerDto = new CustomerDto
        {
            Id = existingCustomer.Id,
            FirstName = "Updated",
            LastName = "Name",
            Email = "updated@example.com",
            PhoneNumber = "555-1234",
            DateOfBirth = new DateTime(1990, 1, 1),
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map(customerDto, existingCustomer);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(existingCustomer); // Should be the same instance
        result.FirstName.Should().Be("Updated");
        result.Email.Should().Be("updated@example.com");
        result.PhoneNumber.Should().Be("555-1234");
        result.Id.Should().Be(existingCustomer.Id); // ID should remain the same
    }
}