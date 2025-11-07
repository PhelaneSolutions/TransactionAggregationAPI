#!/usr/bin/env pwsh

# Transaction Aggregation API - Run Script
Write-Host "🚀 Starting Transaction Aggregation API..." -ForegroundColor Green

# Navigate to project directory
$projectPath = "C:\Users\phela\OneDrive\Desktop\Transaction aggregation API"
Set-Location $projectPath

# Check if .NET is installed
Write-Host "📋 Checking .NET installation..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ .NET SDK is not installed. Please install .NET 8 SDK." -ForegroundColor Red
    exit 1
}
Write-Host "✅ .NET Version: $dotnetVersion" -ForegroundColor Green

# Clean and restore
Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean --verbosity quiet

Write-Host "📦 Restoring packages..." -ForegroundColor Yellow  
dotnet restore --verbosity quiet

# Build the solution
Write-Host "🔨 Building solution..." -ForegroundColor Yellow
dotnet build --no-restore --verbosity quiet

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed. Check the errors above." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build successful!" -ForegroundColor Green

# Start the API
Write-Host "🌐 Starting API server..." -ForegroundColor Yellow
Write-Host "📍 API will be available at: http://localhost:5062" -ForegroundColor Cyan
Write-Host "📖 Swagger UI: http://localhost:5062/swagger" -ForegroundColor Cyan
Write-Host "" 
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Gray

# Run the API
dotnet run --project TransactionAggregationAPI.API