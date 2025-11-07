using TransactionAggregationAPI.Application.DTOs;

namespace TransactionAggregationAPI.Application.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task<AccountDto?> GetAccountByIdAsync(string id);
    Task<IEnumerable<AccountDto>> GetAccountsByCustomerIdAsync(string customerId);
    Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto);
}