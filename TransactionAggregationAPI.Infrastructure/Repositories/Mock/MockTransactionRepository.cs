using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Infrastructure.Repositories.Mock;

public class MockTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions;

    public MockTransactionRepository()
    {
        _transactions = GenerateMockTransactions();
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        await Task.Delay(10);
        return _transactions.OrderByDescending(t => t.TransactionDate).ToList();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        await Task.Delay(10);
        return _transactions.FirstOrDefault(t => t.Id == id);
    }

    public async Task<IEnumerable<Transaction>> GetByCustomerIdAsync(string customerId)
    {
        await Task.Delay(10);
        return _transactions
            .Where(t => t.CustomerId == customerId)
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(string accountId)
    {
        await Task.Delay(10);
        return _transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        await Task.Delay(10);
        return _transactions
            .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }

    public async Task<IEnumerable<Transaction>> GetByCategoryAsync(TransactionCategory category)
    {
        await Task.Delay(10);
        return _transactions
            .Where(t => t.Category == category)
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }

    public async Task<IEnumerable<Transaction>> GetByDataSourceAsync(string dataSource)
    {
        await Task.Delay(10);
        return _transactions
            .Where(t => t.DataSource == dataSource)
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        await Task.Delay(10);
        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;
        transaction.UpdatedAt = DateTime.UtcNow;
        _transactions.Add(transaction);
        return transaction;
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        await Task.Delay(10);
        var existingTransaction = _transactions.FirstOrDefault(t => t.Id == transaction.Id);
        if (existingTransaction != null)
        {
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Currency = transaction.Currency;
            existingTransaction.Description = transaction.Description;
            existingTransaction.MerchantName = transaction.MerchantName;
            existingTransaction.Type = transaction.Type;
            existingTransaction.Category = transaction.Category;
            existingTransaction.Status = transaction.Status;
            existingTransaction.UpdatedAt = DateTime.UtcNow;
            return existingTransaction;
        }
        return null;
    }

    public async Task DeleteAsync(Guid id)
    {
        await Task.Delay(10);
        var transaction = _transactions.FirstOrDefault(t => t.Id == id);
        if (transaction != null)
        {
            _transactions.Remove(transaction);
        }
    }

    public async Task<int> CountAsync()
    {
        await Task.Delay(10);
        return _transactions.Count;
    }

    public async Task<decimal> GetTotalAmountAsync(string? customerId = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        await Task.Delay(10);
        var query = _transactions.AsQueryable();

        if (!string.IsNullOrEmpty(customerId))
        {
            query = query.Where(t => t.CustomerId == customerId);
        }

        if (startDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= endDate.Value);
        }

        return query.Sum(t => t.Amount);
    }

    private List<Transaction> GenerateMockTransactions()
    {
        var random = new Random();
        var transactions = new List<Transaction>();
        var baseDate = DateTime.UtcNow.AddDays(-90);

        var merchants = new[]
        {
            ("Starbucks Coffee", TransactionCategory.Food),
            ("Shell Gas Station", TransactionCategory.Transportation),
            ("Amazon.com", TransactionCategory.Shopping),
            ("Walmart Supercenter", TransactionCategory.Shopping),
            ("McDonald's", TransactionCategory.Food),
            ("Target Store", TransactionCategory.Shopping),
            ("Netflix Subscription", TransactionCategory.Entertainment),
            ("Uber Technologies", TransactionCategory.Transportation),
            ("CVS Pharmacy", TransactionCategory.Healthcare),
            ("AT&T Mobility", TransactionCategory.Bills),
            ("Electric Company", TransactionCategory.Bills),
            ("Whole Foods Market", TransactionCategory.Food),
            ("Exxon Mobile", TransactionCategory.Transportation),
            ("Apple Store", TransactionCategory.Shopping),
            ("Spotify Premium", TransactionCategory.Entertainment),
            ("Doctor's Office", TransactionCategory.Healthcare),
            ("University Tuition", TransactionCategory.Education),
            ("Hotel Marriott", TransactionCategory.Travel),
            ("Delta Airlines", TransactionCategory.Travel),
            ("Gym Membership", TransactionCategory.Healthcare)
        };

        var customers = new[] { "CUST001", "CUST002", "CUST003" };
        var accounts = new[] { "ACC001", "ACC002", "ACC003", "ACC004", "ACC005", "ACC006", "ACC007", "ACC008", "ACC009" };

        // Map customers to their accounts for more realistic data
        var customerAccounts = new Dictionary<string, string[]>
        {
            { "CUST001", new[] { "ACC001", "ACC002", "ACC005" } }, // John: Checking, Savings, Credit
            { "CUST002", new[] { "ACC003", "ACC006", "ACC007" } }, // Jane: Checking, Savings, Investment
            { "CUST003", new[] { "ACC004", "ACC008", "ACC009" } }  // Mike: Business Checking, Business Savings, Loan
        };

        // Generate 120 mock transactions (40 per customer)
        for (int i = 0; i < 120; i++)
        {
            var merchant = merchants[random.Next(merchants.Length)];
            var customerId = customers[i % customers.Length]; // Distribute evenly across customers
            var customerAccountList = customerAccounts[customerId];
            var accountId = customerAccountList[random.Next(customerAccountList.Length)];
            var amount = Math.Round((decimal)(random.NextDouble() * 500 + 10), 2);
            var isCredit = random.Next(10) == 0; // 10% chance of credit transaction

            // For credit accounts (ACC005), make transactions more likely to be charges (debits)
            if (accountId == "ACC005" && random.Next(5) > 0) // 80% chance of debit for credit card
            {
                isCredit = false;
            }

            // For loan accounts (ACC009), only allow payments (credits)
            if (accountId == "ACC009")
            {
                isCredit = true;
                amount = Math.Round((decimal)(random.NextDouble() * 1000 + 200), 2); // Larger loan payments
            }

            transactions.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                AccountId = accountId,
                Amount = isCredit ? amount : -amount,
                Currency = "USD",
                TransactionDate = baseDate.AddDays(random.Next(90)),
                Description = $"Purchase at {merchant.Item1}",
                MerchantName = merchant.Item1,
                Type = isCredit ? TransactionType.Credit : TransactionType.Debit,
                Category = merchant.Item2,
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"TXN{DateTime.UtcNow.Ticks}{i:D3}",
                DataSource = random.Next(2) == 0 ? "BankA" : "CreditUnion",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Add some specific salary/income transactions
        transactions.AddRange(new[]
        {
            new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = "CUST001",
                AccountId = "ACC001",
                Amount = 4500.00m,
                Currency = "USD",
                TransactionDate = DateTime.UtcNow.AddDays(-30),
                Description = "Salary Deposit - Tech Corp",
                MerchantName = "Tech Corp Inc.",
                Type = TransactionType.Credit,
                Category = TransactionCategory.Income,
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"TXN{DateTime.UtcNow.Ticks}SAL1",
                DataSource = "BankA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = "CUST002",
                AccountId = "ACC003",
                Amount = 3800.00m,
                Currency = "USD",
                TransactionDate = DateTime.UtcNow.AddDays(-15),
                Description = "Salary Deposit - Marketing LLC",
                MerchantName = "Marketing LLC",
                Type = TransactionType.Credit,
                Category = TransactionCategory.Income,
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"TXN{DateTime.UtcNow.Ticks}SAL2",
                DataSource = "CreditUnion",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = "CUST003",
                AccountId = "ACC004",
                Amount = 8750.00m,
                Currency = "USD",
                TransactionDate = DateTime.UtcNow.AddDays(-7),
                Description = "Business Revenue - Client Payment",
                MerchantName = "ABC Corporation",
                Type = TransactionType.Credit,
                Category = TransactionCategory.Income,
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"TXN{DateTime.UtcNow.Ticks}BUS1",
                DataSource = "BankA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        });

        // Add some salary deposits
        foreach (var customerId in customers)
        {
            var accountId = accounts[random.Next(accounts.Length)];
            transactions.Add(new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                AccountId = accountId,
                Amount = 5000.00m,
                Currency = "USD",
                TransactionDate = DateTime.UtcNow.AddDays(-30),
                Description = "Salary Deposit",
                MerchantName = "Employer Corp",
                Type = TransactionType.Credit,
                Category = TransactionCategory.Income,
                Status = TransactionStatus.Completed,
                ReferenceNumber = $"SAL{DateTime.UtcNow.Ticks}{customerId}",
                DataSource = "Payroll System",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        return transactions;
    }
}