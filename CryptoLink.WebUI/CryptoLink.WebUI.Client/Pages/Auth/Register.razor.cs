namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Register
    {
        private int step = 1;
        private string username;
        private string publicKey;
        private string encryptedMessage;
        private string decryptedToken;

        private async Task HandleInitiate()
        {
            encryptedMessage = await AuthService.InitiateRegisterAsync(username, publicKey);
            step = 2;
        }

        private async Task HandleComplete()
        {
            // 3. Wysyłamy odszyfrowany token do API
            await AuthService.CompleteRegisterAsync(username, decryptedToken);
            // 4. Sukces - przekierowanie
            Navigation.NavigateTo("/login");
        }
    }
}
