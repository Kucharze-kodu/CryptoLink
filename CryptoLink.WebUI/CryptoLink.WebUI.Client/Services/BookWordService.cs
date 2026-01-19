using CryptoLink.WebUI.Client.Services.Command;
using CryptoLink.WebUI.Client.Services.Dto;
using System.Net.Http.Json;
using System.Text.Json;

public class BookWordService
{
    private readonly HttpClient _httpClient;

    public BookWordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateBookWordAsync(CreateBookWordCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/bookword", command);

        if (response.IsSuccessStatusCode)
        {
            await response.Content.ReadFromJsonAsync<int>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera podczas tworzenia: {errorContent}");
        }
    }

    public async Task<string> GenerateLinkAsync(CreateRandomLinkCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/generateLink", command);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<string>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera podczas tworzenia: {errorContent}");
        }
    }



    public async Task DeleteBookWordAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "api/bookword")
        {
            Content = JsonContent.Create(id)
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera podczas usuwania: {errorContent}");
        }
    }

    public async Task<List<string>> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("api/Categorybookword");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<string>>();
            return result ?? new List<string>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera podczas pobierania kategorii: {errorContent}");
        }
    }

    public async Task<List<BookWordDto>> GetBookWordsAsync(List<string> query)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/bookword")
        {
            Content = JsonContent.Create(query)
        };

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<List<BookWordDto>>();
            return result ?? new List<BookWordDto>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Błąd serwera podczas pobierania słówek: {errorContent}");
        }
    }
}