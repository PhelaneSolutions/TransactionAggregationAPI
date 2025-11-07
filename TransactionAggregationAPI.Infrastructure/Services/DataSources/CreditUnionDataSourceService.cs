using Microsoft.Extensions.Logging;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Infrastructure.Services.DataSources;

public class CreditUnionDataSourceService : IDataSourceService
{
    private readonly ILogger<CreditUnionDataSourceService> _logger;

    public CreditUnionDataSourceService(ILogger<CreditUnionDataSourceService> logger)
    {
        _logger = logger;
    }

    public string DataSourceName => "Credit Union";

    public bool IsHealthy => true;

    public async Task<bool> CheckHealthAsync()
    {
        await Task.Delay(50); // Simulate network call
        _logger.LogInformation("Health check for {DataSource}: OK", DataSourceName);
        return true;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        await Task.Delay(150); // Simulate network call
        
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = "CU_CUST001",
                FirstName = "Sarah",
                LastName = "Connor",
                Email = "sarah.connor@example.com",
                PhoneNumber = "+1234567895",
                DateOfBirth = new DateTime(1980, 7, 12),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddMonths(-18),
                UpdatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Id = "CU_CUST002", 
                FirstName = "David",
                LastName = "Miller",
                Email = "david.miller@example.com",
                PhoneNumber = "+1234567896",
                DateOfBirth = new DateTime(1992, 2, 28),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddMonths(-24),
                UpdatedAt = DateTime.UtcNow
            }
        };

        return customers;
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync(string customerId)
    {
        await Task.Delay(100); // Simulate network call

        var accounts = new Dictionary<string, List<Account>>
        {
            ["CU_CUST001"] = new List<Account>
            {
                new Account
                {
                    Id = "CU_ACC001",
                    CustomerId = customerId,
                    AccountNumber = "5001234567",
                    AccountName = "Sarah Connor Savings",
                    Type = AccountType.Savings,
                    Currency = "USD",
                    Balance = 7800.50m,
                    AvailableBalance = 7800.50m,
                    Status = AccountStatus.Active,
                    OpenedDate = DateTime.UtcNow.AddMonths(-18),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            },
            ["CU_CUST002"] = new List<Account>
            {
                new Account
                {
                    Id = "CU_ACC002",
                    CustomerId = customerId,
                    AccountNumber = "5001234568",
                    AccountName = "David Miller Checking",
                    Type = AccountType.Checking,
                    Currency = "USD",
                    Balance = 2150.75m,
                    AvailableBalance = 2150.75m,
                    Status = AccountStatus.Active,
                    OpenedDate = DateTime.UtcNow.AddMonths(-24),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            }
        };

        return accounts.ContainsKey(customerId) ? accounts[customerId] : new List<Account>();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string customerId, DateTime? startDate = null, DateTime? endDate = null)
    {
        await Task.Delay(250); // Simulate network call

        var baseTransactions = new Dictionary<string, List<Transaction>>
        {
            ["CU_CUST001"] = GenerateTransactionsForCustomer("CU_CUST001", "CU_ACC001"),
            ["CU_CUST002"] = GenerateTransactionsForCustomer("CU_CUST002", "CU_ACC002")
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
        var baseDate = DateTime.UtcNow.AddDays(-45);

        var merchants = new[]
        {
            "Local Coffee Shop", "City Gas Station", "Online Retailer", "Grocery Store",
            "Fast Food Restaurant", "Department Store", "Streaming Service", "Ride Share",
            "Pharmacy Chain", "Telecom Provider", "Fitness Center", "Bookstore"
        };

        for (int i = 0; i < 15; i++)
        {
            var merchant = merchants[random.Next(merchants.Length)];
            var amount = Math.Round((decimal)(random.NextDouble() * 150 + 5), 2);
            
            transactions.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                AccountId = accountId,
                Amount = -amount, // Most transactions are debits
                Currency = "USD",
                TransactionDate = baseDate.AddDays(random.Next(45)),
                Description = $"Purchase at {merchant}",
                MerchantName = merchant,
                Type = TransactionType.Debit,
                Category = TransactionCategory.Unknown, // Will be categorized later
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"CU{DateTime.UtcNow.Ticks}{i:D3}",
                DataSource = DataSourceName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Add a salary deposit
        transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            AccountId = accountId,
            Amount = 3500.00m,
            Currency = "USD",
            TransactionDate = DateTime.UtcNow.AddDays(-15),
            Description = "Direct Deposit - Salary",
            MerchantName = "Employer Inc",
            Type = TransactionType.Credit,
            Category = TransactionCategory.Income,
            Status = TransactionStatus.Completed,
            ReferenceNumber = $"CU_DD{DateTime.UtcNow.Ticks}",
            DataSource = DataSourceName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        return transactions;
    }
}