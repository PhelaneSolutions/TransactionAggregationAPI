using TransactionAggregationAPI.Application.DTOs;

namespace TransactionAggregationAPI.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(string id);
    Task<CustomerDto?> GetCustomerByEmailAsync(string email);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
}