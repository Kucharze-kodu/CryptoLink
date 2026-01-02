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
            var result = await response.Content.ReadFromJsonAsync<RegisterInitResponse>();


            return result.Challenge;
        }
        else
        {
            // Obsługa błędu
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
}