using CryptoLink.WebUI.Client.Services.Command;
using CryptoLink.WebUI.Client.Services.Dto;
using System.Net.Http.Json;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> InitiateRegisterAsync(string username, string publicKey)
    {
        var command = new RegisterInitCommand(username, publicKey);

        var response = await _httpClient.PostAsJsonAsync("api/auth/register/init", command);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<InitResponse>();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize InitResponse from register endpoint.");
            }

            return result.Challenge;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera: {errorContent}");
        }
    }

    public async Task CompleteRegisterAsync(string username, string decryptedToken)
    {
        var command = new { Username = username, DecryptedToken = decryptedToken };

        var response = await _httpClient.PostAsJsonAsync("api/auth/register/complete", command);
        response.EnsureSuccessStatusCode();
    }


    public async Task<string> InitiateLoginAsync(string username)
    {
        var command = new LoginInitCommand(username);

        var response = await _httpClient.PostAsJsonAsync("api/auth/login/init", command);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<InitResponse>();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize InitResponse from login endpoint.");
            }

            return result.Challenge;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera: {errorContent}");
        }
    }

    public async Task CompleteLoginAsync(string username, string decryptedToken)
    {
        var command = new { Username = username, DecryptedToken = decryptedToken };

        var response = await _httpClient.PostAsJsonAsync("api/auth/login/complete", command);

        response.EnsureSuccessStatusCode();
    }
}