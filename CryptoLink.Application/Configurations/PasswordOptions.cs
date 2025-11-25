namespace CryptoLink.Application.Configurations;

public class PasswordOptions
{
    public int ExpiryInDays { get; init; }
    public int MinimumLength { get; init; }
}