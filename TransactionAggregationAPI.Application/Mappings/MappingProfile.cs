using AutoMapper;
using TransactionAggregationAPI.Application.DTOs;
using TransactionAggregationAPI.Domain.Entities;

namespace TransactionAggregationAPI.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Transaction mappings
        CreateMap<Transaction, TransactionDto>();
        CreateMap<TransactionDto, Transaction>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TransactionStatus.Pending))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => TransactionCategory.Unknown));

        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Accounts, opt => opt.Ignore());
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CustomerStatus.Active))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Accounts, opt => opt.Ignore());

        // Account mappings
        CreateMap<Account, AccountDto>();
        CreateMap<AccountDto, Account>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore());
        CreateMap<CreateAccountDto, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => GenerateAccountNumber()))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance))
            .ForMember(dest => dest.AvailableBalance, opt => opt.MapFrom(src => src.InitialBalance))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AccountStatus.Active))
            .ForMember(dest => dest.OpenedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore());
    }

    private static string GenerateAccountNumber()
    {
        return $"ACC{DateTime.UtcNow.Ticks}";
    }
}