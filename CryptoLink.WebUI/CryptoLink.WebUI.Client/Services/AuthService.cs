using CryptoLink.WebUI.Client.Services;
using CryptoLink.WebUI.Client.Services.Command;
using CryptoLink.WebUI.Client.Services.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
    }

    public async Task<string> InitiateRegisterAsync(string username, string publicKey)
    {
        var command = new RegisterInitCommand(username, publicKey);

        var response = await _httpClient.PostAsJsonAsync("api/auth/register/init", command);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<InitResponse>();


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


    public async Task LogoutUser()
    {
        var response = await _httpClient.PostAsync("api/auth/logout", null);
        response.EnsureSuccessStatusCode();
        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            customProvider.NotifyUserLogout();
        }
    }
}