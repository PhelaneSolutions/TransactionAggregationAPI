using AutoMapper;
using Microsoft.Extensions.Logging;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Interfaces;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        _logger.LogInformation("Getting all customers");
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(string id)
    {
        _logger.LogInformation("Getting customer by ID: {CustomerId}", id);
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto?> GetCustomerByEmailAsync(string email)
    {
        _logger.LogInformation("Getting customer by email: {Email}", email);
        var customer = await _customerRepository.GetByEmailAsync(email);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        _logger.LogInformation("Creating new customer with email: {Email}", createCustomerDto.Email);
        
        var customer = _mapper.Map<Customer>(createCustomerDto);
        var createdCustomer = await _customerRepository.CreateAsync(customer);
        
        return _mapper.Map<CustomerDto>(createdCustomer);
    }
}