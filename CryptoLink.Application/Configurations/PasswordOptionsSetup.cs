using CryptoLink.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CryptoLink.Application.Configurations;

public sealed class PasswordOptionsSetup : IConfigureOptions<PasswordOptions>
{
    private const string SectionName = "Password";
    private readonly IConfiguration _configuration;

    public PasswordOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(PasswordOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}