using CryptoLink.WebUI.Client.Services.Command;
using CryptoLink.WebUI.Client.Services.Dto;
using System.Net.Http.Json;



namespace CryptoLink.WebUI.Client.Services
{
    public class LinksService
    {
        private readonly HttpClient _httpClient;
        public LinkExtendedDto? LinkToEdit { get; set; }

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
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/linkExtended");

            request.Content = JsonContent.Create(new { Id = id });

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Błąd podczas usuwania");
            }
        }


        public async Task<List<LinkExtendedDto>?> GetAllLinksAsync()
        {
            var response = await _httpClient.GetAsync("api/linkExtended");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<LinkExtendedDto>>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd pobierania danych: {error}");
            }
        }

        public async Task<LinkExtendedDto?> GetLinksAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/linkExtendedbyId?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LinkExtendedDto>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd pobierania danych: {error}");
            }
        }

        public async Task<LoadLinkDto?> LoadLinkAsync(string queryLink)
        {

            var response = await _httpClient.GetAsync($"api/LoadlinkExtended?link={queryLink}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoadLinkDto>();
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