using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Domain.Interfaces;

public interface IDataSourceService
{
    Task<IEnumerable<Transaction>> GetTransactionsAsync(string customerId, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<IEnumerable<Account>> GetAccountsAsync(string customerId);
    string DataSourceName { get; }
    bool IsHealthy { get; }
    Task<bool> CheckHealthAsync();
}