
namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Login
    {
        private int step = 1;
        private string username;
        private string encryptedMessage;
        private string decryptedToken;
        private string errorMessage = string.Empty; // For displaying errors in UI



        private async Task HandleInit()
        {
            try
            {
                // STEP 1: Fetch the challenge
                encryptedMessage = await AuthService.InitiateLoginAsync(username);
                step = 2;
            }
            catch (Exception ex)
            {
                // E.g., user not found in database
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

            var cleanToken = decryptedToken.Trim();

            try
            {
                await AuthService.CompleteLoginAsync(username, decryptedToken);

                Navigation.NavigateTo("/", forceLoad: true);
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd weryfikacji!");
            }
        }
    }
}
