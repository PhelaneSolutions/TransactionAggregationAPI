using Microsoft.Extensions.Logging;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Infrastructure.Services.DataSources;

public class BankADataSourceService : IDataSourceService
{
    private readonly ILogger<BankADataSourceService> _logger;

    public BankADataSourceService(ILogger<BankADataSourceService> logger)
    {
        _logger = logger;
    }

    public string DataSourceName => "Bank A";

    public bool IsHealthy => true;

    public async Task<bool> CheckHealthAsync()
    {
        await Task.Delay(100); // Simulate network call
        _logger.LogInformation("Health check for {DataSource}: OK", DataSourceName);
        return true;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        await Task.Delay(200); // Simulate network call
        
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = "BANKA_CUST001",
                FirstName = "Alice",
                LastName = "Wilson",
                Email = "alice.wilson@example.com",
                PhoneNumber = "+1234567893",
                DateOfBirth = new DateTime(1982, 3, 10),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddMonths(-6),
                UpdatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Id = "BANKA_CUST002", 
                FirstName = "Bob",
                LastName = "Brown",
                Email = "bob.brown@example.com",
                PhoneNumber = "+1234567894",
                DateOfBirth = new DateTime(1975, 11, 25),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddMonths(-12),
                UpdatedAt = DateTime.UtcNow
            }
        };

        return customers;
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync(string customerId)
    {
        await Task.Delay(150); // Simulate network call

        var accounts = new Dictionary<string, List<Account>>
        {
            ["BANKA_CUST001"] = new List<Account>
            {
                new Account
                {
                    Id = "BANKA_ACC001",
                    CustomerId = customerId,
                    AccountNumber = "3001234567",
                    AccountName = "Alice Wilson Checking",
                    Type = AccountType.Checking,
                    Currency = "USD",
                    Balance = 4200.75m,
                    AvailableBalance = 4200.75m,
                    Status = AccountStatus.Active,
                    OpenedDate = DateTime.UtcNow.AddMonths(-6),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            },
            ["BANKA_CUST002"] = new List<Account>
            {
                new Account
                {
                    Id = "BANKA_ACC002",
                    CustomerId = customerId,
                    AccountNumber = "3001234568",
                    AccountName = "Bob Brown Business",
                    Type = AccountType.Business,
                    Currency = "USD",
                    Balance = 15600.00m,
                    AvailableBalance = 15600.00m,
                    Status = AccountStatus.Active,
                    OpenedDate = DateTime.UtcNow.AddMonths(-12),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            }
        };

        return accounts.ContainsKey(customerId) ? accounts[customerId] : new List<Account>();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string customerId, DateTime? startDate = null, DateTime? endDate = null)
    {
        await Task.Delay(300); // Simulate network call

        var baseTransactions = new Dictionary<string, List<Transaction>>
        {
            ["BANKA_CUST001"] = GenerateTransactionsForCustomer("BANKA_CUST001", "BANKA_ACC001"),
            ["BANKA_CUST002"] = GenerateTransactionsForCustomer("BANKA_CUST002", "BANKA_ACC002")
        };

        if (!baseTransactions.ContainsKey(customerId))
            return new List<Transaction>();

        var transactions = baseTransactions[customerId];

        if (startDate.HasValue)
            transactions = transactions.Where(t => t.TransactionDate >= startDate.Value).ToList();

        if (endDate.HasValue)
            transactions = transactions.Where(t => t.TransactionDate <= endDate.Value).ToList();

        return transactions;
    }

    private List<Transaction> GenerateTransactionsForCustomer(string customerId, string accountId)
    {
        var random = new Random();
        var transactions = new List<Transaction>();
        var baseDate = DateTime.UtcNow.AddDays(-30);

        var merchants = new[]
        {
            "Starbucks Coffee", "Shell Gas Station", "Amazon.com", "Walmart Supercenter",
            "McDonald's", "Target Store", "Netflix Subscription", "Uber Technologies",
            "CVS Pharmacy", "AT&T Mobility"
        };

        for (int i = 0; i < 20; i++)
        {
            var merchant = merchants[random.Next(merchants.Length)];
            var amount = Math.Round((decimal)(random.NextDouble() * 200 + 10), 2);
            
            transactions.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                AccountId = accountId,
                Amount = -amount, // Most transactions are debits
                Currency = "USD",
                TransactionDate = baseDate.AddDays(random.Next(30)),
                Description = $"Purchase at {merchant}",
                MerchantName = merchant,
                Type = TransactionType.Debit,
                Category = TransactionCategory.Unknown, // Will be categorized later
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"BANKA{DateTime.UtcNow.Ticks}{i:D3}",
                DataSource = DataSourceName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        return transactions;
    }
}