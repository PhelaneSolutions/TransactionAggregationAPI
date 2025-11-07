using TransactionAggregationAPI.Application.Interfaces;
using TransactionAggregationAPI.Application.Services;
using TransactionAggregationAPI.Application.Mappings;
using TransactionAggregationAPI.Domain.Interfaces;
using TransactionAggregationAPI.Infrastructure.Repositories.Mock;
using TransactionAggregationAPI.Infrastructure.Services;
using TransactionAggregationAPI.Infrastructure.Services.DataSources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Enable automatic model validation
        options.SuppressModelStateInvalidFilter = false;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddSingleton<ITransactionRepository, MockTransactionRepository>();
builder.Services.AddSingleton<ICustomerRepository, MockCustomerRepository>();
builder.Services.AddSingleton<IAccountRepository, MockAccountRepository>();

// Services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionCategorizationService, TransactionCategorizationService>();

// Data Sources
builder.Services.AddScoped<IDataSourceService, BankADataSourceService>();
builder.Services.AddScoped<IDataSourceService, CreditUnionDataSourceService>();

// Logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
