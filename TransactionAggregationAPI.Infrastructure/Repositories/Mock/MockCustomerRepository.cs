using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Infrastructure.Repositories.Mock;

public class MockCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers;

    public MockCustomerRepository()
    {
        _customers = new List<Customer>
        {
            new Customer
            {
                Id = "CUST001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "+1234567890",
                DateOfBirth = new DateTime(1985, 5, 15),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow,
                Accounts = new List<Account>
                {
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
                    }
                }
            },
            new Customer
            {
                Id = "CUST002",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "+1234567891",
                DateOfBirth = new DateTime(1990, 8, 22),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddMonths(-8),
                UpdatedAt = DateTime.UtcNow,
                Accounts = new List<Account>
                {
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
                    }
                }
            },
            new Customer
            {
                Id = "CUST003",
                FirstName = "Mike",
                LastName = "Johnson",
                Email = "mike.johnson@example.com",
                PhoneNumber = "+1234567892",
                DateOfBirth = new DateTime(1978, 12, 3),
                Status = CustomerStatus.Active,
                CreatedAt = DateTime.UtcNow.AddMonths(-14),
                UpdatedAt = DateTime.UtcNow,
                Accounts = new List<Account>
                {
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
                }
            }
        };

        // Set up circular references
        foreach (var customer in _customers)
        {
            foreach (var account in customer.Accounts)
            {
                account.Customer = customer;
            }
        }
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        await Task.Delay(10); // Simulate async operation
        return _customers.ToList();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        await Task.Delay(10);
        return _customers.FirstOrDefault(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        await Task.Delay(10);
        return _customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await Task.Delay(10);
        customer.Id = $"CUST{_customers.Count + 1:D3}";
        customer.CreatedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;
        customer.Status = CustomerStatus.Active;
        _customers.Add(customer);
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        await Task.Delay(10);
        var existingCustomer = _customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existingCustomer != null)
        {
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.DateOfBirth = customer.DateOfBirth;
            existingCustomer.Status = customer.Status;
            existingCustomer.UpdatedAt = DateTime.UtcNow;
            return existingCustomer;
        }
        return null;
    }

    public async Task DeleteAsync(string id)
    {
        await Task.Delay(10);
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
            _customers.Remove(customer);
        }
    }

    public async Task<int> CountAsync()
    {
        await Task.Delay(10);
        return _customers.Count;
    }
}