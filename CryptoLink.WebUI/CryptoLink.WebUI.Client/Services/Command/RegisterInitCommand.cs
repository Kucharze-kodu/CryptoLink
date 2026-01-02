namespace CryptoLink.WebUI.Client.Services.Command
{
    public record RegisterInitCommand(
        string UserName,
        string PublicKey
        );
}
