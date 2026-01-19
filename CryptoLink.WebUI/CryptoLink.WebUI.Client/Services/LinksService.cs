using CryptoLink.WebUI.Client.Services.Command;
using System.Net.Http.Json;



namespace CryptoLink.WebUI.Client.Services
{
    public class LinksService
    {
        private readonly HttpClient _httpClient;

        public LinksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task CreateLinkAsync(CreateLinkExtendedCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/linkExtended", command);
            await HandleResponse(response);
        }


        public async Task EditLinkAsync(EditLinkExtendedCommand command)
        {
            var response = await _httpClient.PutAsJsonAsync("api/linkExtended", command);
            await HandleResponse(response);
        }

        public async Task DeleteLinkAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/linkExtended")
            {
                Content = JsonContent.Create(id)
            };

            var response = await _httpClient.SendAsync(request);
            await HandleResponse(response);
        }


        public async Task<TResult?> GetAllLinksAsync<TResult>()
        {
            var response = await _httpClient.GetAsync("api/linkExtended");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd pobierania danych: {error}");
            }
        }


        public async Task<TResult?> LoadLinkAsync<TResult>(string queryLink)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/LoadlinkExtended")
            {
                Content = JsonContent.Create(queryLink)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd ładowania linku: {error}");
            }
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd serwera: {errorContent}");
            }
        }
    }
}