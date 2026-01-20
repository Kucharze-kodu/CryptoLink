using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace CryptoLink.WebUI.Client.Services
{
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CookieAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                Console.WriteLine("CookieAuthenticationStateProvider: Checking authentication state...");
                
                // Endpoint, który serwer zwraca informacje o zalogowanym użytkowniku
                var userInfo = await _httpClient.GetFromJsonAsync<UserInfo>("/api/auth/me");

                if (userInfo != null)
                {
                    Console.WriteLine($"CookieAuthenticationStateProvider: User authenticated: {userInfo.Username}");
                    
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                        new Claim(ClaimTypes.Name, userInfo.Username)
                    };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }
                
                Console.WriteLine("CookieAuthenticationStateProvider: No user info returned");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CookieAuthenticationStateProvider error: {ex.Message}");
            }

            return new AuthenticationState(new ClaimsPrincipal());
        }

        public void NotifyUserAuthentication(string username, string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }

    public class UserInfo
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
