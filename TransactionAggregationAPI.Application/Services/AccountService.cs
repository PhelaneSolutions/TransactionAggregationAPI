using AutoMapper;
using Microsoft.Extensions.Logging;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Application.Interfaces;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IAccountRepository accountRepository,
        IMapper mapper,
        ILogger<AccountService> logger)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        _logger.LogInformation("Getting all accounts");
        var accounts = await _accountRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }

    public async Task<AccountDto?> GetAccountByIdAsync(string id)
    {
        _logger.LogInformation("Getting account by ID: {AccountId}", id);
        var account = await _accountRepository.GetByIdAsync(id);
        return account != null ? _mapper.Map<AccountDto>(account) : null;
    }

    public async Task<IEnumerable<AccountDto>> GetAccountsByCustomerIdAsync(string customerId)
    {
        _logger.LogInformation("Getting accounts for customer: {CustomerId}", customerId);
        var accounts = await _accountRepository.GetByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
    {
        _logger.LogInformation("Creating new account for customer: {CustomerId}", createAccountDto.CustomerId);
        
        var account = _mapper.Map<Account>(createAccountDto);
        var createdAccount = await _accountRepository.CreateAsync(account);
        
        return _mapper.Map<AccountDto>(createdAccount);
    }
}