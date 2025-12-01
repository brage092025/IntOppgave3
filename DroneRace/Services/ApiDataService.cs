using System;
using System.Net.Http.Json;

namespace DroneRace.Services;

public class ApiDataService
{
    private readonly HttpClient _http;

    public ApiDataService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiData?> GetDataAsync()
    {
        var rndAlbumId = new Random().Next(1, 100);
        var response = await _http.GetAsync($"https://jsonplaceholder.typicode.com/albums/{rndAlbumId}");
        
        if (!response.IsSuccessStatusCode)
            throw new Exception("Remote API returned an error");

        return await response.Content.ReadFromJsonAsync<ApiData>();
    }
}


public class ApiData
{
    public int Id { get; set; }
    public string? Title { get; set; }
}


