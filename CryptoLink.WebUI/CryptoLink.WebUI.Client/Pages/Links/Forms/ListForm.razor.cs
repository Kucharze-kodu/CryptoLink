using CryptoLink.WebUI.Client.Services;
using CryptoLink.WebUI.Client.Services.Command;
using CryptoLink.WebUI.Client.Services.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace CryptoLink.WebUI.Client.Pages.Links.Forms
{
    partial class ListForm
    {

        public LinkExtendedDto LinkModel { get; set; } = new LinkExtendedDto();

        private bool IsEditMode = false;
        private bool isLoading = true;
        private string errorMessage = "";
        private int? linkIdToEdit = null;

        protected override async Task OnInitializedAsync()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("Action", out var action) && action == "edit")
            {
                IsEditMode = true;

                if (queryParams.TryGetValue("IdLink", out var idStr) && int.TryParse(idStr, out int id))
                {
                    linkIdToEdit = id;

                    if (LinksService.LinkToEdit != null && LinksService.LinkToEdit.Id == id)
                    {
                        LinkModel = LinksService.LinkToEdit;

                        LinksService.LinkToEdit = null; 
                        isLoading = false;
                    }
                    else
                    {
                        await LoadLinkData(id);
                    }
                }
            }
            else
            {
                IsEditMode = false;
                LinkModel = new LinkExtendedDto { ExpiretOnUtc = DateTime.UtcNow.AddDays(7) };
                isLoading = false;
            }
        }

        private async Task LoadLinkData(int id)
        {
            try
            {
                isLoading = true;
                var linkFromApi = await LinksService.GetLinksAsync(id);

                if (linkFromApi != null)
                {
                    LinkModel = new LinkExtendedDto
                    {
                        Id = linkFromApi.Id,
                        UrlExtended = linkFromApi.UrlExtended,
                        UrlShort = linkFromApi.UrlShort,
                        CreatedOnUtc = linkFromApi.CreatedOnUtc,
                        ExpiretOnUtc = linkFromApi.ExpiretOnUtc
                    };
                }
                else
                {
                    errorMessage = "Nie znaleziono linku o podanym ID.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Błąd podczas ładowania danych: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task HandleValidSubmit()
        {
            errorMessage = "";

            try
            {
                if (IsEditMode && linkIdToEdit.HasValue)
                {
                    var command = new EditLinkExtendedCommand(
                        Id: linkIdToEdit.Value,
                        UrlExtended: LinkModel.UrlExtended, 
                        DataExpire: LinkModel.ExpiretOnUtc.ToUniversalTime()
                    );

                    await LinksService.EditLinkAsync(command);
                }

                Navigation.NavigateTo("/ListLink");
            }
            catch (Exception ex)
            {
                errorMessage = $"Wystąpił błąd podczas zapisywania: {ex.Message}";
            }
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/ListLink");
        }
    }
}