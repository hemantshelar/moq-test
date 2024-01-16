using System.Text.Json.Nodes;
using JokeGenerator.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace JokeGenerator.Services;

public interface IJokeProvider
{
    string GetJoke();
    string GetJokes(int number);
    Task<List<string>> GetJokeCategories();

    void TestMocqVerify(string message, int id);
}

public class JokeProvider(
    IHttpClientFactory _httpClientFactory,
    IOptions<JokesAPI> _options,
    ILogger<JokeProvider> _looger,
    ICustomLogger _customLogger
    ) : IJokeProvider
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
        _customLogger.WriteLog("test",1);
        _looger.LogInformation("->");
        string[] response = [];
        var httpClient = _httpClientFactory.CreateClient(AppConstants.CNJokeGenerator);
        var res = await httpClient.GetAsync(_options.Value.CNJokeGeneratorAPIOperations.Categories);
        var strResponse = await res.Content.ReadAsStringAsync();
        var jc = JsonSerializer.Deserialize<List<string>>(strResponse);
        _looger.LogInformation("<-");
        return jc;
    }

    public string GetJokes(int number)
    {
        var httpClient = _httpClientFactory.CreateClient(AppConstants.CNJokeGenerator);
        throw new NotImplementedException();
    }

    public void TestMocqVerify(string message, int id)
    {
        Console.WriteLine($"message is {message} {id}");
    }
}