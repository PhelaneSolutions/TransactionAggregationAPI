using Microsoft.Extensions.Logging;
using TransactionAggregationAPI.Domain.Entities;
using TransactionAggregationAPI.Domain.Interfaces;

namespace TransactionAggregationAPI.Infrastructure.Services;

public class TransactionCategorizationService : ITransactionCategorizationService
{
    private readonly ILogger<TransactionCategorizationService> _logger;

    public TransactionCategorizationService(ILogger<TransactionCategorizationService> logger)
    {
        _logger = logger;
    }

    public async Task<TransactionCategory> CategorizeTransactionAsync(Transaction transaction)
    {
        _logger.LogInformation("Categorizing transaction {TransactionId}", transaction.Id);

        var category = await Task.Run(() => CategorizeByDescription(transaction));
        
        _logger.LogInformation("Transaction {TransactionId} categorized as {Category}", transaction.Id, category);
        
        return category;
    }

    public async Task<IEnumerable<Transaction>> CategorizeTransactionsAsync(IEnumerable<Transaction> transactions)
    {
        _logger.LogInformation("Categorizing {Count} transactions", transactions.Count());

        var categorizedTransactions = new List<Transaction>();

        foreach (var transaction in transactions)
        {
            transaction.Category = await CategorizeTransactionAsync(transaction);
            categorizedTransactions.Add(transaction);
        }

        return categorizedTransactions;
    }

    private TransactionCategory CategorizeByDescription(Transaction transaction)
    {
        var description = transaction.Description?.ToLowerInvariant() ?? "";
        var merchantName = transaction.MerchantName?.ToLowerInvariant() ?? "";
        var combinedText = $"{description} {merchantName}";

        // Food and dining
        if (ContainsAny(combinedText, "restaurant", "cafe", "food", "pizza", "burger", "starbucks", "mcdonalds", "subway", "grocery", "supermarket", "walmart", "kroger"))
            return TransactionCategory.Food;

        // Transportation
        if (ContainsAny(combinedText, "uber", "lyft", "taxi", "gas", "fuel", "exxon", "shell", "chevron", "bp", "parking", "metro", "bus", "train", "airline"))
            return TransactionCategory.Transportation;

        // Entertainment
        if (ContainsAny(combinedText, "netflix", "spotify", "movie", "cinema", "theater", "concert", "game", "steam", "playstation", "xbox"))
            return TransactionCategory.Entertainment;

        // Shopping
        if (ContainsAny(combinedText, "amazon", "ebay", "target", "bestbuy", "mall", "store", "shop", "clothing", "fashion"))
            return TransactionCategory.Shopping;

        // Bills and utilities
        if (ContainsAny(combinedText, "electric", "electricity", "water", "gas bill", "phone", "internet", "cable", "utility", "bill", "payment"))
            return TransactionCategory.Bills;

        // Healthcare
        if (ContainsAny(combinedText, "hospital", "doctor", "pharmacy", "cvs", "walgreens", "medical", "health", "clinic"))
            return TransactionCategory.Healthcare;

        // Education
        if (ContainsAny(combinedText, "school", "university", "college", "tuition", "education", "book", "student"))
            return TransactionCategory.Education;

        // Travel
        if (ContainsAny(combinedText, "hotel", "airbnb", "booking", "expedia", "travel", "vacation", "flight"))
            return TransactionCategory.Travel;

        // Investment
        if (ContainsAny(combinedText, "investment", "stock", "bond", "mutual fund", "etf", "dividend", "broker"))
            return TransactionCategory.Investment;

        // Income
        if (transaction.Amount > 0 && ContainsAny(combinedText, "salary", "payroll", "wage", "deposit", "income", "bonus"))
            return TransactionCategory.Income;

        // Transfer
        if (ContainsAny(combinedText, "transfer", "wire", "ach", "p2p", "venmo", "paypal", "zelle"))
            return TransactionCategory.Transfer;

        // Fee
        if (ContainsAny(combinedText, "fee", "charge", "overdraft", "atm", "service charge", "maintenance"))
            return TransactionCategory.Fee;

        // Insurance
        if (ContainsAny(combinedText, "insurance", "premium", "auto insurance", "health insurance", "life insurance"))
            return TransactionCategory.Insurance;

        // Charity
        if (ContainsAny(combinedText, "donation", "charity", "church", "foundation", "nonprofit"))
            return TransactionCategory.Charity;

        // Default to Other if no match found
        return TransactionCategory.Other;
    }

    private static bool ContainsAny(string text, params string[] keywords)
    {
        return keywords.Any(keyword => text.Contains(keyword));
    }
}