namespace CryptoLink.WebUI.Client.Services.Command
{
    public record CreateRandomLinkCommand
    (
        int HowManyWord,
        List<string> Categories
        );
}
