using CryptoLink.WebUI.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Nodes;


namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Login
    {

        private int step = 1;
        private string username;
        private string encryptedMessage;
        private string decryptedToken;
        private string errorMessage = string.Empty;


        private async Task HandleInit()
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "Proszę podać nazwę użytkownika.";
                return;
            }

            try
            {
                encryptedMessage = await AuthService.InitiateLoginAsync(username);
                step = 2;
            }
            catch (Exception ex)
            {
                errorMessage = "Błąd: " + ex.Message;
                Console.WriteLine(ex.Message);

                errorMessage = ExtractErrorMessage(ex.Message);

            }
        }

        private async Task HandleVerify()
        {
            errorMessage = string.Empty;

            try
            {
                await AuthService.CompleteLoginAsync(username, decryptedToken);
                Navigation.NavigateTo("/", forceLoad: true);
            }
            catch (Exception ex)
            {
                errorMessage = ExtractErrorMessage(ex.Message);
            }
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
                        if (detail == "User not found") return "Użytkownik nie został znaleziony.";
                        if (detail == "Invalid token") return "Podano nieprawidłowy token.";

                        return "źle odszyfrowany token";
                    }
                }
            }
            catch
            {

            }

            return rawMessage.Replace("Błąd inicjalizacji: ", "").Replace("Błąd serwera: ", "");
        }
    }
}
