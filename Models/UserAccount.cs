namespace CueVault.Models;

/// <summary>A registered CueVault customer and their password hash material.</summary>
public record UserAccount(string Username, string Email, byte[] Salt, byte[] Hash);