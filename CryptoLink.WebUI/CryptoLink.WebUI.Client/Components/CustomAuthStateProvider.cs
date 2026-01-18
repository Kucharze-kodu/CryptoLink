using CryptoLink.WebUI.Client.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("API");
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        try
        {
            var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>("/api/authC/user-info");

            if (userInfo.IsAuthenticated)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userInfo.UserName),
                    new Claim(ClaimTypes.Role, userInfo.Role)
                }, "CookieAuth");
            }
        }
        catch {  }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}

