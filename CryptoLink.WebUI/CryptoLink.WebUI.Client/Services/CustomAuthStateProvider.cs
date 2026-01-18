using System.Net.Http.Json;
using System.Security.Claims;
using CryptoLink.WebUI.Client.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace CryptoLink.WebUI.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void NotifyUserLogout()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {

                var userDto = await _httpClient.GetFromJsonAsync<UserSessionDto>("api/auth/me");

                // 2. Jeśli API zwróciło dane - budujemy tożsamość użytkownika
                if (userDto != null && !string.IsNullOrEmpty(userDto.Name))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userDto.Name),
                    };

                    var identity = new ClaimsIdentity(claims, "ServerAuth");
                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }
            }
            catch (Exception)
            {
                // Jeśli API zwróci błąd (np. 401 Unauthorized)
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}