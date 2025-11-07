using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Domain.Interfaces;

public interface ITransactionCategorizationService
{
    Task<TransactionCategory> CategorizeTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> CategorizeTransactionsAsync(IEnumerable<Transaction> transactions);
}