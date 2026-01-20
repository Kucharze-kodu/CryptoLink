using CryptoLink.WebUI.Client.Services;
using Microsoft.AspNetCore.Components;

namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Login
    {
        [Inject]
        private CookieAuthenticationStateProvider AuthProvider { get; set; }

        private int step = 1;
        private string username;
        private string encryptedMessage;
        private string decryptedToken;
        private string errorMessage = string.Empty;

        private async Task HandleInit()
        {
            try
            {
                encryptedMessage = await AuthService.InitiateLoginAsync(username);
                step = 2;
            }
            catch (Exception ex)
            {
                errorMessage = "Błąd: " + ex.Message;
                Console.WriteLine(ex.Message);
            }
        }

        private async Task HandleVerify()
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(decryptedToken))
            {
                errorMessage = "Wklej odszyfrowany token.";
                return;
            }

            try
            {
                await AuthService.CompleteLoginAsync(username, decryptedToken);
                Console.WriteLine("Login successful, notifying auth provider...");

                // Powiadom AuthenticationStateProvider
                AuthProvider.NotifyUserAuthentication(username, username);
                Console.WriteLine("Auth provider notified, navigating...");

                // Nawiguj bez forceLoad aby pozostać w Blazor kontekście
                Navigation.NavigateTo("/");
            }
            catch (Exception ex)
            {
                errorMessage = "Błąd logowania: " + ex.Message;
                Console.WriteLine("Error verify: " + ex.Message);
            }
        }
    }
}
