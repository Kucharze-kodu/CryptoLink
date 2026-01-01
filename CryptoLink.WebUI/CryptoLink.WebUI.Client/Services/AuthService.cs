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
        var command = new { Username = username, PublicKey = publicKey };

        var response = await _httpClient.PostAsJsonAsync("api/auth/register/init", command);
        response.EnsureSuccessStatusCode();

        // API zwraca zaszyfrowany ciąg znaków
        return await response.Content.ReadAsStringAsync();
    }

    public async Task CompleteRegisterAsync(string username, string decryptedToken)
    {
        var command = new { Username = username, DecryptedToken = decryptedToken };

        var response = await _httpClient.PostAsJsonAsync("api/auth/register/complete", command);
        response.EnsureSuccessStatusCode();
    }
}