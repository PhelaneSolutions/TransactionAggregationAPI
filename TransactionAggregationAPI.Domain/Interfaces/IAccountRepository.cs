using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Domain.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account?> GetByIdAsync(string id);
    Task<IEnumerable<Account>> GetByCustomerIdAsync(string customerId);
    Task<Account> CreateAsync(Account account);
    Task<Account> UpdateAsync(Account account);
    Task DeleteAsync(string id);
    Task<int> CountAsync();
}