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
- **Data Annotations**: Comprehensive input validation with custom business rules
- **Swagger/OpenAPI**: API documentation
- **xUnit**: Testing framework

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
