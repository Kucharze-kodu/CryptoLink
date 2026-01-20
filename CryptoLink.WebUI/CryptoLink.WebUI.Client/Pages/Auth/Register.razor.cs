

using System.Text.Json.Nodes;

namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Register
    {
        private int step = 1;
        private string username = string.Empty;
        private string publicKey = string.Empty;
        private string encryptedMessage = string.Empty;
        private string decryptedToken = string.Empty;
        private string errorMessage = string.Empty;

        private async Task HandleInitiate()
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(publicKey))
            {
                errorMessage = "Wypełnij wszystkie pola (nazwa użytkownika i klucz publiczny).";
                return;
            }

            try
            {
                encryptedMessage = await AuthService.InitiateRegisterAsync(username, publicKey);
                decryptedToken = string.Empty;
                step = 2;
            }
            catch (Exception ex)
            {
                errorMessage = ExtractErrorMessage(ex.Message);
            }
        }

        private async Task HandleComplete()
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(decryptedToken))
            {
                errorMessage = "Wklej odszyfrowany token, aby potwierdzić posiadanie klucza.";
                return;
            }

            var cleanToken = decryptedToken.Trim();

            try
            {
                await AuthService.CompleteRegisterAsync(username, cleanToken);

                ClearSensitiveData();

                Navigation.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                errorMessage = ExtractErrorMessage(ex.Message);
            }
        }

        private void ResetRegister()
        {
            step = 1;
            errorMessage = string.Empty;
            decryptedToken = string.Empty;
            encryptedMessage = string.Empty;
        }

        private void ClearSensitiveData()
        {
            publicKey = string.Empty;
            encryptedMessage = string.Empty;
            decryptedToken = string.Empty;
        }


        private string ExtractErrorMessage(string rawMessage)
        {
            try
            {
                int jsonStartIndex = rawMessage.IndexOf('{');
                if (jsonStartIndex >= 0)
                {
                    string jsonPart = rawMessage.Substring(jsonStartIndex);
                    var jsonNode = JsonNode.Parse(jsonPart);
                    var detail = jsonNode?["detail"]?.ToString();

                    if (!string.IsNullOrEmpty(detail))
                    {
                        if (detail.Contains("User already exists", StringComparison.OrdinalIgnoreCase))
                            return "Taka nazwa użytkownika jest już zajęta.";

                        if (detail.Contains("Invalid public key", StringComparison.OrdinalIgnoreCase))
                            return "Podany klucz publiczny jest nieprawidłowy lub uszkodzony.";

                        if (detail.Contains("Verification failed", StringComparison.OrdinalIgnoreCase))
                            return "Weryfikacja nieudana. Upewnij się, że poprawnie odszyfrowałeś token.";

                        return "źle odszyfrowany token";
                    }
                }
            }
            catch
            {

            }

            return rawMessage
                .Replace("Błąd inicjacji: ", "")
                .Replace("System.Exception: ", "");
        }
    }
}