using Microsoft.AspNetCore.Components;
using CryptoLink.WebUI.Client.Services;

namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Login
    {
        [Inject]
        private AuthService AuthService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

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

                // Powiadom AuthenticationStateProvider
                AuthProvider.NotifyUserAuthentication(username, username);

                Navigation.NavigateTo("/", forceLoad: true);
            }
            catch (Exception ex)
            {
                errorMessage = "Błąd logowania: " + ex.Message;
                Console.WriteLine("Error verify: " + ex.Message);
            }
        }
    }
}
