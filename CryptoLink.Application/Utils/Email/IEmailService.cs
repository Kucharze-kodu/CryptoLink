namespace CryptoLink.Application.Utils.Email;

public interface IEmailService
{
    Task<string> SendEmailWithVerificationTokenAsync(
        string email,
        string firstName,
        string verificationToken,
        string verifyEmailUrl,
        CancellationToken cancellationToken = default);
    Task<string> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
    
}