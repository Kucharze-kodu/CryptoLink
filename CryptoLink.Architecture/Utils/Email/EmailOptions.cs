namespace CryptoLink.Architecture.Utils.Email;

public class EmailOptions
{
    public string ApiKeyPublic { get; init; } = null!;
    public string ApiKeyPrivate { get; init; } = null!;
    public string FromEmail { get; init; } = null!;
    public string FromName { get; init; } = null!;
    
}