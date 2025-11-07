using FluentAssertions;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Customer_Should_Initialize_With_Valid_Properties()
    {
        // Arrange & Act
        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "555-1234",
            DateOfBirth = new DateTime(1990, 1, 1),
            Status = CustomerStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        customer.Id.Should().NotBeNullOrEmpty();
        customer.FirstName.Should().Be("John");
        customer.LastName.Should().Be("Doe");
        customer.Email.Should().Be("john.doe@example.com");
        customer.PhoneNumber.Should().Be("555-1234");
        customer.DateOfBirth.Should().Be(new DateTime(1990, 1, 1));
        customer.Status.Should().Be(CustomerStatus.Active);
        customer.Accounts.Should().NotBeNull();
        customer.Accounts.Should().BeEmpty();
    }

    [Fact]
    public void Customer_Should_Allow_Adding_Accounts()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Status = CustomerStatus.Active
        };

        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = customer.Id,
            AccountNumber = "123456789",
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = 1000.00m,
            Status = AccountStatus.Active
        };

        // Act
        customer.Accounts.Add(account);

        // Assert
        customer.Accounts.Should().HaveCount(1);
        customer.Accounts.First().Should().Be(account);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Customer_Should_Handle_Invalid_Names_Gracefully(string invalidName)
    {
        // Arrange & Act
        var customer = new Customer
        {
            FirstName = invalidName,
            LastName = "Doe"
        };

        // Assert - The entity itself doesn't enforce validation, that's handled at the application layer
        customer.FirstName.Should().Be(invalidName);
    }
}