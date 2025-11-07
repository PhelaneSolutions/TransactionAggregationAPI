using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Infrastructure.Repositories.Mock;

public class MockAccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts;

    public MockAccountRepository()
    {
        _accounts = new List<Account>
        {
            // John Doe's Accounts (CUST001)
            new Account
            {
                Id = "ACC001",
                CustomerId = "CUST001",
                AccountNumber = "1001234567",
                AccountName = "John Doe Checking",
                Type = AccountType.Checking,
                Currency = "USD",
                Balance = 5500.00m,
                AvailableBalance = 5500.00m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddYears(-2),
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow
            },
            new Account
            {
                Id = "ACC002",
                CustomerId = "CUST001",
                AccountNumber = "2001234567",
                AccountName = "John Doe Savings",
                Type = AccountType.Savings,
                Currency = "USD",
                Balance = 12000.00m,
                AvailableBalance = 12000.00m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddYears(-1),
                CreatedAt = DateTime.UtcNow.AddYears(-1),
                UpdatedAt = DateTime.UtcNow
            },
            new Account
            {
                Id = "ACC005",
                CustomerId = "CUST001",
                AccountNumber = "3001234567",
                AccountName = "John Doe Credit Card",
                Type = AccountType.Credit,
                Currency = "USD",
                Balance = -1250.75m,
                AvailableBalance = 3749.25m, // $5000 limit - $1250.75 used
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-18),
                CreatedAt = DateTime.UtcNow.AddMonths(-18),
                UpdatedAt = DateTime.UtcNow
            },

            // Jane Smith's Accounts (CUST002)
            new Account
            {
                Id = "ACC003",
                CustomerId = "CUST002",
                AccountNumber = "1001234568",
                AccountName = "Jane Smith Checking",
                Type = AccountType.Checking,
                Currency = "USD",
                Balance = 3200.50m,
                AvailableBalance = 3200.50m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-8),
                CreatedAt = DateTime.UtcNow.AddMonths(-8),
                UpdatedAt = DateTime.UtcNow
            },
            new Account
            {
                Id = "ACC006",
                CustomerId = "CUST002",
                AccountNumber = "2001234568",
                AccountName = "Jane Smith Savings",
                Type = AccountType.Savings,
                Currency = "USD",
                Balance = 7500.25m,
                AvailableBalance = 7500.25m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-6),
                CreatedAt = DateTime.UtcNow.AddMonths(-6),
                UpdatedAt = DateTime.UtcNow
            },
            new Account
            {
                Id = "ACC007",
                CustomerId = "CUST002",
                AccountNumber = "4001234568",
                AccountName = "Jane Smith Investment",
                Type = AccountType.Investment,
                Currency = "USD",
                Balance = 15000.00m,
                AvailableBalance = 15000.00m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-4),
                CreatedAt = DateTime.UtcNow.AddMonths(-4),
                UpdatedAt = DateTime.UtcNow
            },

            // Mike Johnson's Accounts (CUST003)
            new Account
            {
                Id = "ACC004",
                CustomerId = "CUST003",
                AccountNumber = "1001234569",
                AccountName = "Mike Johnson Business Checking",
                Type = AccountType.Business,
                Currency = "USD",
                Balance = 8750.25m,
                AvailableBalance = 8750.25m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-14),
                CreatedAt = DateTime.UtcNow.AddMonths(-14),
                UpdatedAt = DateTime.UtcNow
            },
            new Account
            {
                Id = "ACC008",
                CustomerId = "CUST003",
                AccountNumber = "2001234569",
                AccountName = "Mike Johnson Business Savings",
                Type = AccountType.Savings,
                Currency = "USD",
                Balance = 25000.00m,
                AvailableBalance = 25000.00m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-12),
                CreatedAt = DateTime.UtcNow.AddMonths(-12),
                UpdatedAt = DateTime.UtcNow
            },
            new Account
            {
                Id = "ACC009",
                CustomerId = "CUST003",
                AccountNumber = "5001234569",
                AccountName = "Mike Johnson Business Loan",
                Type = AccountType.Loan,
                Currency = "USD",
                Balance = -45000.00m,
                AvailableBalance = 0.00m,
                Status = AccountStatus.Active,
                OpenedDate = DateTime.UtcNow.AddMonths(-10),
                CreatedAt = DateTime.UtcNow.AddMonths(-10),
                UpdatedAt = DateTime.UtcNow
            }
        };
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        await Task.Delay(10);
        return _accounts.ToList();
    }

    public async Task<Account?> GetByIdAsync(string id)
    {
        await Task.Delay(10);
        return _accounts.FirstOrDefault(a => a.Id == id);
    }

    public async Task<IEnumerable<Account>> GetByCustomerIdAsync(string customerId)
    {
        await Task.Delay(10);
        return _accounts.Where(a => a.CustomerId == customerId).ToList();
    }

    public async Task<Account> CreateAsync(Account account)
    {
        await Task.Delay(10);
        account.Id = $"ACC{_accounts.Count + 1:D3}";
        account.AccountNumber = $"100123456{_accounts.Count + 1}";
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;
        account.Status = AccountStatus.Active;
        account.OpenedDate = DateTime.UtcNow;
        _accounts.Add(account);
        return account;
    }

    public async Task<Account> UpdateAsync(Account account)
    {
        await Task.Delay(10);
        var existingAccount = _accounts.FirstOrDefault(a => a.Id == account.Id);
        if (existingAccount != null)
        {
            existingAccount.AccountName = account.AccountName;
            existingAccount.Type = account.Type;
            existingAccount.Currency = account.Currency;
            existingAccount.Balance = account.Balance;
            existingAccount.AvailableBalance = account.AvailableBalance;
            existingAccount.Status = account.Status;
            existingAccount.UpdatedAt = DateTime.UtcNow;
            return existingAccount;
        }
        return null;
    }

    public async Task DeleteAsync(string id)
    {
        await Task.Delay(10);
        var account = _accounts.FirstOrDefault(a => a.Id == id);
        if (account != null)
        {
            _accounts.Remove(account);
        }
    }

    public async Task<int> CountAsync()
    {
        await Task.Delay(10);
        return _accounts.Count;
    }
}