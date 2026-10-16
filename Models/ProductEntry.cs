namespace CueVault.Models;

/// <summary>A billiards product available in the CueVault catalog.</summary>
public record ProductEntry(
    string Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    int Stock,
    string Condition,
    DateTime CreatedAt,
    string[] Tags);