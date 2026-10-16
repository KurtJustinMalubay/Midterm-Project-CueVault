namespace CueVault.Models;

/// <summary>A customer's review of a CueVault product.</summary>
public record ReviewEntry(
    string Id,
    string ProductId,
    string ProductName,
    string Author,
    int Rating,
    string Comment,
    DateTime CreatedAt);