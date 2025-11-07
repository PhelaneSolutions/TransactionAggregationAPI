using AutoMapper;
using Microsoft.Extensions.Logging;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Interfaces;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCategorizationService _categorizationService;
    private readonly IEnumerable<IDataSourceService> _dataSourceServices;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        ITransactionRepository transactionRepository,
        ITransactionCategorizationService categorizationService,
        IEnumerable<IDataSourceService> dataSourceServices,
        IMapper mapper,
        ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
        _categorizationService = categorizationService;
        _dataSourceServices = dataSourceServices;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
    {
        _logger.LogInformation("Getting all transactions");
        var transactions = await _transactionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto?> GetTransactionByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting transaction by ID: {TransactionId}", id);
        var transaction = await _transactionRepository.GetByIdAsync(id);
        return transaction != null ? _mapper.Map<TransactionDto>(transaction) : null;
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByCustomerIdAsync(string customerId)
    {
        _logger.LogInformation("Getting transactions for customer: {CustomerId}", customerId);
        var transactions = await _transactionRepository.GetByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(string accountId)
    {
        _logger.LogInformation("Getting transactions for account: {AccountId}", accountId);
        var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Getting transactions for date range: {StartDate} to {EndDate}", startDate, endDate);
        var transactions = await _transactionRepository.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByCategoryAsync(TransactionCategory category)
    {
        _logger.LogInformation("Getting transactions for category: {Category}", category);
        var transactions = await _transactionRepository.GetByCategoryAsync(category);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
    {
        _logger.LogInformation("Creating new transaction for customer: {CustomerId}", createTransactionDto.CustomerId);
        
        var transaction = _mapper.Map<Transaction>(createTransactionDto);
        
        // Categorize the transaction
        transaction.Category = await _categorizationService.CategorizeTransactionAsync(transaction);
        
        var createdTransaction = await _transactionRepository.CreateAsync(transaction);
        return _mapper.Map<TransactionDto>(createdTransaction);
    }

    public async Task<TransactionSummaryDto> GetTransactionSummaryAsync(string? customerId = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        _logger.LogInformation("Getting transaction summary for customer: {CustomerId}, date range: {StartDate} to {EndDate}", 
            customerId, startDate, endDate);

        IEnumerable<Transaction> transactions;

        if (!string.IsNullOrEmpty(customerId))
        {
            transactions = await _transactionRepository.GetByCustomerIdAsync(customerId);
        }
        else
        {
            transactions = await _transactionRepository.GetAllAsync();
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);
        }

        var transactionList = transactions.ToList();
        var totalAmount = transactionList.Sum(t => t.Amount);

        var summary = new TransactionSummaryDto
        {
            TotalTransactions = transactionList.Count,
            TotalAmount = totalAmount,
            AverageAmount = transactionList.Count > 0 ? totalAmount / transactionList.Count : 0,
            CategoryCounts = transactionList.GroupBy(t => t.Category).ToDictionary(g => g.Key, g => g.Count()),
            CategoryAmounts = transactionList.GroupBy(t => t.Category).ToDictionary(g => g.Key, g => g.Sum(t => t.Amount)),
            MonthlyTransactionCounts = transactionList.GroupBy(t => t.TransactionDate.ToString("yyyy-MM")).ToDictionary(g => g.Key, g => g.Count()),
            MonthlyAmounts = transactionList.GroupBy(t => t.TransactionDate.ToString("yyyy-MM")).ToDictionary(g => g.Key, g => g.Sum(t => t.Amount))
        };

        return summary;
    }

    public async Task AggregateTransactionsFromDataSourcesAsync()
    {
        _logger.LogInformation("Starting transaction aggregation from data sources");

        foreach (var dataSource in _dataSourceServices)
        {
            try
            {
                if (!await dataSource.CheckHealthAsync())
                {
                    _logger.LogWarning("Data source {DataSourceName} is not healthy, skipping", dataSource.DataSourceName);
                    continue;
                }

                _logger.LogInformation("Aggregating transactions from data source: {DataSourceName}", dataSource.DataSourceName);

                // Get customers to fetch their transactions
                var customers = await dataSource.GetCustomersAsync();
                
                foreach (var customer in customers)
                {
                    var transactions = await dataSource.GetTransactionsAsync(customer.Id);
                    var categorizedTransactions = await _categorizationService.CategorizeTransactionsAsync(transactions);

                    foreach (var transaction in categorizedTransactions)
                    {
                        // Check if transaction already exists to avoid duplicates
                        var existingTransaction = await _transactionRepository.GetByIdAsync(transaction.Id);
                        if (existingTransaction == null)
                        {
                            await _transactionRepository.CreateAsync(transaction);
                        }
                    }
                }

                _logger.LogInformation("Completed aggregation from data source: {DataSourceName}", dataSource.DataSourceName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aggregating transactions from data source: {DataSourceName}", dataSource.DataSourceName);
            }
        }

        _logger.LogInformation("Completed transaction aggregation from all data sources");
    }
}