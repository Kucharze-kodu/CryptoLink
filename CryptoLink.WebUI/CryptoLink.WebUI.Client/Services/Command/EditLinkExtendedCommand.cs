namespace CryptoLink.WebUI.Client.Services.Command
{
    public record EditLinkExtendedCommand
    (
        int Id,
        string UrlExtended,
        DateTime DataExpire
        );
}
