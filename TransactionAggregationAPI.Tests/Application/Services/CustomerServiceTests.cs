using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Services;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Tests.Application.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CustomerService>> _mockLogger;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CustomerService>>();
        _customerService = new CustomerService(_mockCustomerRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllCustomersAsync_Should_Return_All_Customers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = "cust1", FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" },
            new Customer { Id = "cust2", FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "0987654321" }
        };

        var customerDtos = new List<CustomerDto>
        {
            new CustomerDto { Id = "cust1", FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" },
            new CustomerDto { Id = "cust2", FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "0987654321" }
        };

        _mockCustomerRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(customers);
        _mockMapper.Setup(x => x.Map<IEnumerable<CustomerDto>>(customers)).Returns(customerDtos);

        // Act
        var result = await _customerService.GetAllCustomersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(customerDtos);
        _mockCustomerRepository.Verify(x => x.GetAllAsync(), Times.Once);
        _mockMapper.Verify(x => x.Map<IEnumerable<CustomerDto>>(customers), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_Should_Return_Customer_When_Found()
    {
        // Arrange
        var customerId = "cust1";
        var customer = new Customer { Id = customerId, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" };
        var customerDto = new CustomerDto { Id = customerId, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" };

        _mockCustomerRepository.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _mockMapper.Setup(x => x.Map<CustomerDto>(customer)).Returns(customerDto);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customerDto);
        _mockCustomerRepository.Verify(x => x.GetByIdAsync(customerId), Times.Once);
        _mockMapper.Verify(x => x.Map<CustomerDto>(customer), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var customerId = "cust1";
        _mockCustomerRepository.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(customerId);

        // Assert
        result.Should().BeNull();
        _mockCustomerRepository.Verify(x => x.GetByIdAsync(customerId), Times.Once);
        _mockMapper.Verify(x => x.Map<CustomerDto>(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task CreateCustomerAsync_Should_Create_And_Return_Customer()
    {
        // Arrange
        var createCustomerDto = new CreateCustomerDto 
        { 
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        var customer = new Customer 
        { 
            Id = "cust1",
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        var createdCustomerDto = new CustomerDto 
        { 
            Id = customer.Id,
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john@example.com",
            PhoneNumber = "+1234567890",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        _mockMapper.Setup(x => x.Map<Customer>(createCustomerDto)).Returns(customer);
        _mockCustomerRepository.Setup(x => x.CreateAsync(customer)).ReturnsAsync(customer);
        _mockMapper.Setup(x => x.Map<CustomerDto>(customer)).Returns(createdCustomerDto);

        // Act
        var result = await _customerService.CreateCustomerAsync(createCustomerDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdCustomerDto);
        _mockMapper.Verify(x => x.Map<Customer>(createCustomerDto), Times.Once);
        _mockCustomerRepository.Verify(x => x.CreateAsync(It.IsAny<Customer>()), Times.Once);
        _mockMapper.Verify(x => x.Map<CustomerDto>(customer), Times.Once);
    }


}