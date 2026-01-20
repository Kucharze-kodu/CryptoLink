using Microsoft.AspNetCore.Components.Authorization;
using CryptoLink.WebUI.Client.Services;
using CryptoLink.WebUI.Client.Services.Command;
using CryptoLink.WebUI.Client.Services.Dto;

namespace CryptoLink.WebUI.Client.Pages.Links
{
    partial class CreateLink
    {
        // Stan formularza
        private string userProvidedUrl = "";
        private HashSet<string> selectedCategories = new HashSet<string>();
        private DateTime selectedDate = DateTime.Today.AddDays(7);
        private int wordCount = 10;

        // Stan UI
        private List<string> categories = new List<string>();
        private string generatedLink = "";
        private string message = "";
        private bool isBusy = false;
        private bool isError = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                categories = await BookWordService.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                ShowMessage($"Nie udało się pobrać kategorii: {ex.Message}", true);
            }
        }

        private void UpdateCategories(string category, object? isChecked)
        {
            if (isChecked is bool checkedValue && checkedValue)
            {
                selectedCategories.Add(category);
            }
            else
            {
                selectedCategories.Remove(category);
            }
        }

        private async Task GenerateLink()
        {
            message = "";
            isError = false;
            generatedLink = Navigation.BaseUri+"Link/";


            if (!selectedCategories.Any())
            {
                ShowMessage("Musisz wybrać przynajmniej jedną kategorię, aby wygenerować link!", true);
                return;
            }

            isBusy = true; 

            try
            {
                var command = new CreateRandomLinkCommand(
                    HowManyWord: wordCount,
                    Categories: selectedCategories.ToList()
                );

                generatedLink += await BookWordService.GenerateLinkAsync(command);
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd generowania linku", true);
            }
            finally
            {
                isBusy = false; 
            }
        }

        private async Task SaveLink()
        {
            if (string.IsNullOrWhiteSpace(userProvidedUrl))
            {
                ShowMessage("Pole z linkiem nie może być puste.", true);
                return;
            }
            if (!userProvidedUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !userProvidedUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                userProvidedUrl = "https://" + userProvidedUrl;
            }

            bool isValidUrl = Uri.TryCreate(userProvidedUrl, UriKind.Absolute, out Uri? uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                              && uriResult.Host.Contains(".");
            if (!isValidUrl)
            {
                ShowMessage("Podany tekst nie jest poprawnym linkiem (upewnij się, że zaczyna się od http:// lub https://).", true);
                return;
            }

            if (string.IsNullOrEmpty(generatedLink) && generatedLink != (Navigation.BaseUri+"Link/"))
            {
                ShowMessage("Najpierw wygeneruj link.", true);
                return;
            }

            isBusy = true;
            message = "";

            try
            {
                var command = new CreateLinkExtendedCommand(
                    UrlExtended: generatedLink, 
                    UrlShort: userProvidedUrl,         
                    DataExpire: selectedDate.ToUniversalTime());

                await LinksService.CreateLinkAsync(command);

                ShowMessage("Link został pomyślnie zapisany w bazie!", false);


            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd zapisu: {ex.Message}", true);
            }
            finally
            {
                isBusy = false;
            }
        }

        private void ShowMessage(string msg, bool error)
        {
            message = msg;
            isError = error;
        }
    }
}