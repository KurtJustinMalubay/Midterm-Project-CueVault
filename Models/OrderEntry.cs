namespace CueVault.Models;

/// <summary>A customer's purchase of a catalog item.</summary>
public record OrderEntry(
    string Id,
    string CustomerName,
    string CustomerEmail,
    string ProductId,
    string ProductName,
    int Quantity,
    decimal Total,
    string Status,
    DateTime CreatedAt);