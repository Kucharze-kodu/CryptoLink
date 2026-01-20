using CryptoLink.WebUI.Client.Services.Dto;
using Microsoft.AspNetCore.Components;

namespace CryptoLink.WebUI.Client.Pages
{
    partial class Link
    {
        [Parameter]
        public string? LinkCode { get; set; }

        private bool IsLoading { get; set; } = false;
        private string? ErrorMessage { get; set; }
        private string? RedirectMessage { get; set; } 

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(LinkCode))
            {
                await FetchAndRedirect(LinkCode);
            }
        }

        private async Task FetchAndRedirect(string code)
        {
            IsLoading = true;
            ErrorMessage = null;
            RedirectMessage = null;

            try
            {
                LoadLinkDto targetUrl = await LinksService.LoadLinkAsync(Navigation.BaseUri+"Link/"+code);

                if (!string.IsNullOrEmpty(targetUrl.Url))
                {
                    RedirectMessage = $"Przekierowywanie do: {targetUrl}...";


                    await Task.Delay(1000);

                    Navigation.NavigateTo(targetUrl.Url, forceLoad: true);
                }
                else
                {
                    ErrorMessage = "Link jest pusty lub nie istnieje.";
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Nie udało się pobrać linku:";
                IsLoading = false;
            }
        }

        private void ResetState()
        {
            LinkCode = null;
            ErrorMessage = null;
            RedirectMessage = null;
            Navigation.NavigateTo("/");
        }
    }
}