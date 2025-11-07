using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<IEnumerable<Transaction>> GetByCustomerIdAsync(string customerId);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(string accountId);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetByCategoryAsync(TransactionCategory category);
    Task<IEnumerable<Transaction>> GetByDataSourceAsync(string dataSource);
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<Transaction> UpdateAsync(Transaction transaction);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<decimal> GetTotalAmountAsync(string? customerId = null, DateTime? startDate = null, DateTime? endDate = null);
}