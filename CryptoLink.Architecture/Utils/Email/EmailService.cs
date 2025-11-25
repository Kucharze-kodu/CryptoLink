using CryptoLink.Application.Utils.Email;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace CryptoLink.Architecture.Utils.Email;

public sealed class EmailService : IEmailService
{
    private readonly MailjetClient _client;
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
        _client = new MailjetClient(
            _emailOptions.ApiKeyPublic,
            _emailOptions.ApiKeyPrivate
        );
    }
    
    public async Task<string> SendEmailWithVerificationTokenAsync(
        string email,
        string firstName,
        string verificationToken,
        string verifyEmailUrl,
        CancellationToken cancellationToken = default)
    {
        var message = new EmailMessage(
            "Verify your email",
            "Welcome to CryptoLink",
            $$"""
                <h1>Welcome to CryptoLink</h1>
                <p>Hi {{firstName}},</p>
                <p>Thank you for registering on CryptoLink. 
                Please verify your email address by pass this code: 
                {{verificationToken}}</p>
                Or click the button below to verify your email address:</p>
                <a href="{{verifyEmailUrl}}?email={{email}}&verificationCode={{verificationToken}}" 
                   style="display: inline-block; background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;">
                    Verify Email
                </a>
                """,
            email);
        
        return await SendEmailAsync(message);
    }

    public async Task<string> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var request = new MailjetRequest
        {
            Resource = Send.Resource
        };
        
        request
            .Property(Send.FromEmail, _emailOptions.FromEmail)
            .Property(Send.FromName, _emailOptions.FromName)
            .Property(Send.Subject, emailMessage.Subject)
            .Property(Send.TextPart, emailMessage.TextPart)
            .Property(Send.HtmlPart, emailMessage.HtmlPart)
            .Property(Send.Recipients, new JArray
            {
                new JObject
                {
                    { "Email", emailMessage.ToEmail }
                }
            });
        
        var response = await _client.PostAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            return $"Total: {response.GetTotal()}, Count: {response.GetCount()}\n Message: {response.GetData()}";
        }
        else
        {
            return
                $"StatusCode: {response.StatusCode}\n Error: {response.GetErrorInfo()}\n ErrorMessage: {response.GetErrorMessage()}";
        }
    }
}