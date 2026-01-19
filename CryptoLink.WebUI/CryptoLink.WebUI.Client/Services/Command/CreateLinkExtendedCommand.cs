namespace CryptoLink.WebUI.Client.Services.Command
{
    public record CreateLinkExtendedCommand
    (
        string UrlExtended,
        string UrlShort,
        DateTime DataExpire
        );
}
