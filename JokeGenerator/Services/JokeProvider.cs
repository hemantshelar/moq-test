using System.Text.Json.Nodes;
using JokeGenerator.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace JokeGenerator.Services;

public interface IJokeProvider
{
    string GetJoke();
    string GetJokes(int number);
    Task<List<string>> GetJokeCategories();
}

public class JokeProvider(IHttpClientFactory _httpClientFactory) : IJokeProvider
{
    public string GetJoke()
    {
        var result = "Here is joke!";
        var httpClient = _httpClientFactory.CreateClient(AppConstants.CNJokeGenerator);
        Console.WriteLine(httpClient.BaseAddress.ToString());
        return result;
    }

    public async Task<List<string>> GetJokeCategories()
    {
        string [] response = [];
        var httpClient = _httpClientFactory.CreateClient(AppConstants.CNJokeGenerator);
        var res = await httpClient.GetAsync("categories");
        var strResponse = await res.Content.ReadAsStringAsync();
        var jc = JsonSerializer.Deserialize<List<string>>(strResponse);
        return jc;
    }

    public string GetJokes(int number)
    {
        var httpClient = _httpClientFactory.CreateClient(AppConstants.CNJokeGenerator);
        throw new NotImplementedException();
    }
}