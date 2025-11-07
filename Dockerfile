# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["TransactionAggregationAPI.API/TransactionAggregationAPI.API.csproj", "TransactionAggregationAPI.API/"]
COPY ["TransactionAggregationAPI.Application/TransactionAggregationAPI.Application.csproj", "TransactionAggregationAPI.Application/"]
COPY ["TransactionAggregationAPI.Domain/TransactionAggregationAPI.Domain.csproj", "TransactionAggregationAPI.Domain/"]
COPY ["TransactionAggregationAPI.Infrastructure/TransactionAggregationAPI.Infrastructure.csproj", "TransactionAggregationAPI.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "TransactionAggregationAPI.API/TransactionAggregationAPI.API.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/TransactionAggregationAPI.API"
RUN dotnet build "TransactionAggregationAPI.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TransactionAggregationAPI.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published application
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Entry point
ENTRYPOINT ["dotnet", "TransactionAggregationAPI.API.dll"]