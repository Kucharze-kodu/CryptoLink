using CryptoLink.WebUI.Client.Services;
using CryptoLink.WebUI.Client.Services.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities; 
using Microsoft.JSInterop; 

namespace CryptoLink.WebUI.Client.Pages.Links 
{
    partial class ListLinks 
    {
        private List<LinkExtendedDto>? linkExtended;
        private string message = "";
        private bool isBusy = false;
        private bool isError = false;

        protected override async Task OnInitializedAsync()
        {
            isBusy = true;
            try
            {
                linkExtended = await LinksService.GetAllLinksAsync();
            }
            catch (Exception ex)
            {
                ShowMessage($"Nie udało się pobrać Linków: {ex.Message}", true);
            }
            finally
            {
                isBusy = false;
            }
        }

        private void EditLink(LinkExtendedDto link) 
        {
            LinksService.LinkToEdit = link;

            string action = "edit";


            var url = QueryHelpers.AddQueryString("/ListForm", new Dictionary<string, string?>
            {
                { "IdLink", link.Id.ToString() },
                { "Action", action }
            });

            Navigation.NavigateTo(url);
        }

        private async Task DeleteLink(int linkId)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Czy na pewno chcesz usunąć ten link?");
            if (!confirmed) return;

            try
            {
                await LinksService.DeleteLinkAsync(linkId);

                // Usuń element z lokalnej listy, aby nie trzeba było przeładowywać API
                var itemToRemove = linkExtended?.FirstOrDefault(x => x.Id == linkId);
                if (itemToRemove != null)
                {
                    linkExtended?.Remove(itemToRemove);
                }

                ShowMessage("Link został usunięty.", false);
                StateHasChanged(); 
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd podczas usuwania: {ex.Message}", true);
            }
        }

        private async Task CopyLink(string linkUrl)
        {
            try
            {

                await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", linkUrl);
                ShowMessage("Link skopiowany do schowka!", false);
            }
            catch (Exception)
            {
                ShowMessage("Nie udało się skopiować linku (blokada przeglądarki?).", true);
            }
        }

        private void ShowMessage(string msg, bool error)
        {
            message = msg;
            isError = error;

            if (!error)
            {
                Task.Delay(3000).ContinueWith(_ =>
                {
                    message = "";
                    InvokeAsync(StateHasChanged);
                });
            }
        }
    }
}