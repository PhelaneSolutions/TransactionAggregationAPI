# Transaction Aggregation API

A production-grade Transaction Aggregation API built with .NET 8, implementing Clean Architecture principles for aggregating customer financial transaction data from multiple mock data sources with automatic transaction categorization.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── TransactionAggregationAPI.Domain/          # Core business entities and interfaces
├── TransactionAggregationAPI.Application/     # Use cases, business logic, DTOs
├── TransactionAggregationAPI.Infrastructure/  # Data access, external services, repositories
├── TransactionAggregationAPI.API/            # Controllers, middleware, configuration
└── TransactionAggregationAPI.Tests/          # Unit and integration tests
```

## 🚀 Features

### Core Functionality
- **Transaction Aggregation**: Collect transactions from multiple mock data sources
- **Automatic Categorization**: Intelligent categorization of transactions (Food, Transportation, Entertainment, etc.)
- **Multi-Customer Support**: Manage multiple customers and their accounts
- **Comprehensive APIs**: Extensive REST API endpoints for querying and managing data

### Technical Features
- **Clean Architecture**: Domain-driven design with proper dependency inversion
- **In-Memory Data**: Mock repositories with realistic sample data
- **AutoMapper**: Automatic DTO mapping
- **Comprehensive Validation**: Data annotations with custom business rules
- **Swagger/OpenAPI**: Interactive API documentation
- **Logging**: Structured logging throughout the application
- **Production-Ready**: Error handling, input validation, and proper HTTP status codes

## 🛠️ Technology Stack

- **.NET 8**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API framework
- **Mock Data Sources**: In-memory repositories with 100+ realistic transactions
- **AutoMapper**: Object-to-object mapping
- **Data Annotations**: Comprehensive input validation with custom business rules
- **Swagger/OpenAPI**: API documentation
- **xUnit**: Testing framework

## 📊 API Endpoints

### Transactions
- `GET /api/transactions` - Get all transactions
- `GET /api/transactions/{id}` - Get transaction by ID
- `GET /api/transactions/customer/{customerId}` - Get transactions by customer
- `GET /api/transactions/account/{accountId}` - Get transactions by account
- `GET /api/transactions/daterange?startDate={date}&endDate={date}` - Get transactions by date range
- `GET /api/transactions/category/{category}` - Get transactions by category
- `GET /api/transactions/summary` - Get aggregated transaction summary
- `POST /api/transactions` - Create new transaction
- `POST /api/transactions/aggregate` - Trigger data source aggregation

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/customers/email/{email}` - Get customer by email
- `POST /api/customers` - Create new customer

### Accounts
- `GET /api/accounts` - Get all accounts
- `GET /api/accounts/{id}` - Get account by ID
- `GET /api/accounts/customer/{customerId}` - Get accounts by customer
- `GET /api/accounts/type/{type}` - Get accounts by type (Checking, Savings, Credit, etc.)
- `GET /api/accounts/summary` - Get account summary with statistics
- `POST /api/accounts` - Create new account

## 🏃‍♂️ Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio Code (recommended)
- Git

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd transaction-aggregation-api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project TransactionAggregationAPI.API
   ```

5. **Access the API**
   - Swagger UI: `http://localhost:5062/swagger`
   - API Base URL: `http://localhost:5062/api`

### Data Storage
The application uses in-memory mock repositories with pre-populated sample data including:
- **3 Customers** with different profiles and account types
- **9 Accounts** (checking, savings, credit, investment, business, and loan accounts)
- **120+ Transactions** across various categories and time periods
- **Multiple Data Sources** (BankA and CreditUnion services)

Data is loaded at application startup and provides realistic financial scenarios for testing and demonstration purposes.

## 🧪 Testing

### Run Tests
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Sample API Calls

**Get all transactions:**
```bash
curl -X GET "http://localhost:5062/api/transactions" -H "accept: application/json"
```

**Get transaction summary:**
```bash
curl -X GET "http://localhost:5062/api/transactions/summary" -H "accept: application/json"
```

**Get all accounts:**
```bash
curl -X GET "http://localhost:5062/api/accounts" -H "accept: application/json"
```

**Get customer's accounts:**
```bash
curl -X GET "http://localhost:5062/api/accounts/customer/CUST001" -H "accept: application/json"
```

**Get account summary:**
```bash
curl -X GET "http://localhost:5062/api/accounts/summary" -H "accept: application/json"
```

**Create new account:**
```bash
curl -X POST "http://localhost:5062/api/accounts" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "CUST001",
    "accountName": "John Doe Investment Account",
    "type": 3,
    "currency": "USD",
    "initialBalance": 5000.00
  }'
```

**Trigger transaction aggregation:**
```bash
curl -X POST "http://localhost:5062/api/transactions/aggregate" -H "accept: application/json"
```

## 🐳 Docker Support

### Build Docker Image
```bash
docker build -t transaction-aggregation-api .
```

### Run with Docker
```bash
docker run -p 5062:8080 transaction-aggregation-api
```

### Docker Compose
```bash
docker-compose up -d
```

## 📁 Project Structure

### Domain Layer (`TransactionAggregationAPI.Domain`)
- **Entities**: Core business entities (Transaction, Customer, Account)
- **Enums**: Transaction types, categories, statuses
- **Interfaces**: Repository and service abstractions

### Application Layer (`TransactionAggregationAPI.Application`)
- **DTOs**: Data Transfer Objects for API contracts
- **Interfaces**: Service interfaces
- **Services**: Business logic implementation
- **Mappings**: AutoMapper profiles

### Infrastructure Layer (`TransactionAggregationAPI.Infrastructure`)
- **Repositories**: Mock repository implementations with in-memory data
- **Services**: External service implementations (data sources, categorization)
- **Data Sources**: Multiple mock financial institution services

### API Layer (`TransactionAggregationAPI.API`)
- **Controllers**: REST API endpoints
- **Program.cs**: Application configuration and dependency injection

## 🔍 Data Sources

The application includes multiple mock data source services that simulate external financial institutions:

- **Bank A Data Source**: Simulates transactions from a major commercial bank
- **Credit Union Data Source**: Simulates transactions from a credit union
- **Extensible Architecture**: Easy to add new data sources by implementing `IDataSourceService`
- **Realistic Data**: Generated transactions include various categories, merchants, and amounts

## 🏷️ Transaction Categorization

Automatic transaction categorization based on:
- Merchant names
- Transaction descriptions
- Amount patterns
- Predefined keyword matching

### Supported Categories
- Food & Dining
- Transportation
- Entertainment
- Shopping
- Bills & Utilities
- Healthcare
- Education
- Travel
- Investment
- Income
- Transfers
- Fees
- Insurance
- Charity
- Other

## 🔒 Configuration

### Application Settings
The application uses `appsettings.json` for configuration:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Logging
Configured for structured logging with different levels for development and production. No database configuration needed as the application uses in-memory mock data.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📋 Development Guidelines

- Follow Clean Architecture principles
- Implement proper error handling and logging
- Write comprehensive unit tests
- Use dependency injection throughout
- Follow RESTful API design patterns
- Maintain clean, documented code

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🏆 Production Readiness

This API demonstrates production-grade development practices including:

- ✅ **Clean Architecture**: Proper separation of concerns
- ✅ **SOLID Principles**: Maintainable and extensible code
- ✅ **Dependency Injection**: Loosely coupled components
- ✅ **Input Validation**: Comprehensive data validation with business rules
- ✅ **Error Handling**: Proper exception handling and HTTP status codes
- ✅ **Logging**: Structured logging throughout the application
- ✅ **Testing**: Comprehensive unit and integration test framework
- ✅ **Documentation**: Swagger/OpenAPI specification
- ✅ **Mock Data Architecture**: In-memory repositories with realistic sample data
- ✅ **Configuration**: Environment-based configuration management
- ✅ **Docker**: Containerization support

## 🚧 Future Enhancements

- Authentication & Authorization (JWT tokens)
- Real-time transaction notifications
- Advanced analytics and reporting
- Integration with real financial APIs
- Caching layer (Redis)
- Message queuing (RabbitMQ/Azure Service Bus)
- Microservices decomposition
- GraphQL API support