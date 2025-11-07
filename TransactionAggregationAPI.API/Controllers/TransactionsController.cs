using Microsoft.AspNetCore.Mvc;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Interfaces;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Get all transactions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    /// <summary>
    /// Get transaction by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
    {
        var transaction = await _transactionService.GetTransactionByIdAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }
        return Ok(transaction);
    }

    /// <summary>
    /// Get transactions by customer ID
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByCustomer(string customerId)
    {
        var transactions = await _transactionService.GetTransactionsByCustomerIdAsync(customerId);
        return Ok(transactions);
    }

    /// <summary>
    /// Get transactions by account ID
    /// </summary>
    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByAccount(string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
        {
            return BadRequest(new { Message = "Account ID is required" });
        }

        var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
        return Ok(transactions);
    }

    /// <summary>
    /// Get transactions by date range
    /// </summary>
    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByDateRange(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        if (endDate < startDate)
        {
            return BadRequest(new { Message = "End date must be after start date" });
        }

        var transactions = await _transactionService.GetTransactionsByDateRangeAsync(startDate, endDate);
        return Ok(transactions);
    }

    /// <summary>
    /// Get transactions by category
    /// </summary>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByCategory(TransactionCategory category)
    {
        var transactions = await _transactionService.GetTransactionsByCategoryAsync(category);
        return Ok(transactions);
    }

    /// <summary>
    /// Create a new transaction
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] CreateTransactionDto createTransactionDto)
    {
        var transaction = await _transactionService.CreateTransactionAsync(createTransactionDto);
        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }

    /// <summary>
    /// Get transaction summary with aggregated data
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<TransactionSummaryDto>> GetTransactionSummary(
        [FromQuery] string? customerId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var summary = await _transactionService.GetTransactionSummaryAsync(customerId, startDate, endDate);
        return Ok(summary);
    }

    /// <summary>
    /// Trigger aggregation of transactions from all data sources
    /// </summary>
    [HttpPost("aggregate")]
    public async Task<ActionResult> AggregateTransactions()
    {
        await _transactionService.AggregateTransactionsFromDataSourcesAsync();
        return Ok(new { message = "Transaction aggregation completed successfully" });
    }
}