using Microsoft.AspNetCore.Mvc;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Interfaces;

namespace TransactionAggregationAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    /// <summary>
    /// Get all accounts
    /// </summary>
    /// <returns>List of all accounts in the system</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
    {
        try
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all accounts");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get account by ID
    /// </summary>
    /// <param name="id">Account ID</param>
    /// <returns>Account details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { Message = "Account ID is required" });
            }

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound(new { Message = $"Account with ID '{id}' not found" });
            }

            return Ok(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving account {AccountId}", id);
            return StatusCode(500, new { Message = "Internal server error", Details = "An unexpected error occurred while retrieving the account" });
        }
    }

    /// <summary>
    /// Get all accounts for a specific customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>List of customer's accounts</returns>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsByCustomer(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return BadRequest("Customer ID is required");
            }

            var accounts = await _accountService.GetAccountsByCustomerIdAsync(customerId);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving accounts for customer {CustomerId}", customerId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create a new account
    /// </summary>
    /// <param name="createAccountDto">Account creation details</param>
    /// <returns>Created account details</returns>
    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountDto createAccountDto)
    {
        try
        {
            if (createAccountDto == null)
            {
                return BadRequest(new { Message = "Account data is required" });
            }

            // Model validation is handled by ModelValidationFilter
            var account = await _accountService.CreateAccountAsync(createAccountDto);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument when creating account for customer {CustomerId}", createAccountDto?.CustomerId);
            return BadRequest(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation when creating account for customer {CustomerId}", createAccountDto?.CustomerId);
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account for customer {CustomerId}", createAccountDto?.CustomerId);
            return StatusCode(500, new { Message = "Internal server error", Details = "An unexpected error occurred while creating the account" });
        }
    }

    /// <summary>
    /// Get account summary statistics
    /// </summary>
    /// <returns>Account summary with counts and totals</returns>
    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetAccountSummary()
    {
        try
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            var accountsList = accounts.ToList();

            var summary = new
            {
                TotalAccounts = accountsList.Count,
                AccountsByType = accountsList
                    .GroupBy(a => a.Type)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),
                AccountsByStatus = accountsList
                    .GroupBy(a => a.Status)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),
                TotalBalance = accountsList.Sum(a => a.Balance),
                TotalAvailableBalance = accountsList.Sum(a => a.AvailableBalance),
                AccountsByCustomer = accountsList
                    .GroupBy(a => a.CustomerId)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating account summary");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get accounts by type
    /// </summary>
    /// <param name="type">Account type (Checking, Savings, Credit, Investment, Loan, Business)</param>
    /// <returns>List of accounts of the specified type</returns>
    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsByType(string type)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return BadRequest("Account type is required");
            }

            if (!Enum.TryParse<Domain.Entities.AccountType>(type, true, out var accountType))
            {
                return BadRequest($"Invalid account type. Valid types: {string.Join(", ", Enum.GetNames<Domain.Entities.AccountType>())}");
            }

            var allAccounts = await _accountService.GetAllAccountsAsync();
            var filteredAccounts = allAccounts.Where(a => a.Type == accountType);

            return Ok(filteredAccounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving accounts by type {AccountType}", type);
            return StatusCode(500, "Internal server error");
        }
    }
}