
namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Login
    {
        private int step = 1;
        private string username;
        private string encryptedMessage;
        private string decryptedToken;
        private string errorMessage = string.Empty; // Do wyświetlania błędów w UI



        private async Task HandleInit()
        {
            try
            {
                // KROK 1: Pobierz zagadkę
                encryptedMessage = await AuthService.InitiateLoginAsync(username);
                step = 2;
            }
            catch (Exception ex)
            {
                // Np. brak użytkownika w bazie
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

                Navigation.NavigateTo("/", forceLoad: true);
            }
            catch (Exception)
            {
                Console.WriteLine("Error veryfy");
            }
        }
    }
}
