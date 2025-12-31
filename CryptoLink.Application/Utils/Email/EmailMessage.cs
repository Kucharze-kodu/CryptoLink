namespace CryptoLink.Application.Utils.Email;

public record EmailMessage(
    string Subject,
    string TextPart,
    string HtmlPart,
    string ToEmail);