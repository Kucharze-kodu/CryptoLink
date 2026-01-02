

namespace CryptoLink.WebUI.Client.Pages.Auth
{
    public partial class Register
    {
        private int step = 1;
        private string username = string.Empty;
        private string publicKey = string.Empty;
        private string encryptedMessage = string.Empty;
        private string decryptedToken = string.Empty;
        private string errorMessage = string.Empty; // Do wyświetlania błędów w UI

        private async Task HandleInitiate()
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(publicKey))
            {
                errorMessage = "Wypełnij wszystkie pola.";
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
                errorMessage = $"Błąd inicjacji: {ex.Message}";
            }


        }

        private async Task HandleComplete()
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(decryptedToken))
            {
                errorMessage = "Wklej odszyfrowany token.";
                return;
            }

            // 3. WYCZYSZCZENIE: Tokeny często kopiują się ze spacją na końcu!
            // To jest najważniejszy moment czyszczenia przed wysyłką.
            var cleanToken = decryptedToken.Trim();

            try
            {
                await AuthService.CompleteRegisterAsync(username, cleanToken);

                // 4. Wyczyszczenie wrażliwych danych z pamięci przed nawigacją
                // (Opcjonalne, bo komponent i tak zniknie, ale dobra praktyka security)
                ClearSensitiveData();

                Navigation.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                errorMessage = "Weryfikacja nieudana. Sprawdź czy poprawnie odszyfrowałeś treść.";
                // Nie czyścimy tokenu, żeby użytkownik mógł poprawić literówkę bez wklejania od nowa
            }
        }

        // Metoda pomocnicza do zera
        private void ClearSensitiveData()
        {
            publicKey = string.Empty;
            encryptedMessage = string.Empty;
            decryptedToken = string.Empty;
            // username zostawiamy, może się przydać do logowania
        }
    }
}