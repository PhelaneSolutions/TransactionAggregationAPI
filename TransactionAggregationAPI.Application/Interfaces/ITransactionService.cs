using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
    Task<TransactionDto?> GetTransactionByIdAsync(Guid id);
    Task<IEnumerable<TransactionDto>> GetTransactionsByCustomerIdAsync(string customerId);
    Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(string accountId);
    Task<IEnumerable<TransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<TransactionDto>> GetTransactionsByCategoryAsync(TransactionCategory category);
    Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto);
    Task<TransactionSummaryDto> GetTransactionSummaryAsync(string? customerId = null, DateTime? startDate = null, DateTime? endDate = null);
    Task AggregateTransactionsFromDataSourcesAsync();
}