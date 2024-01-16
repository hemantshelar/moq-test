using JokeGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JokeGenerator.Models;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace JokeGenerator;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Configuration.AddJsonFile("AppSettings.json");

        builder.Services
        .AddOptions<JokesAPI>()
        .Bind(builder.Configuration.GetSection("JokesAPI"));

        var jokesApiConfig = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JokesAPI>>();

        #region Configure DI container
        builder.Services.AddTransient<IJokeProvider,JokeProvider>();
        builder.Services.AddHttpClient(AppConstants.CNJokeGenerator, client => {
            var baseAddress = jokesApiConfig.Value.EndPoint;
            client.BaseAddress = new Uri(baseAddress);
        });
        //builder.Services.AddLogging();
        builder.Services.AddTransient<ICustomLogger,CustomLogger>();
        #endregion
        
        IHost host = builder.Build();
        var jokeProvider = host.Services.GetRequiredService<IJokeProvider>();
        var joke = jokeProvider.GetJoke();
        var categories = await  jokeProvider.GetJokeCategories();
        Console.WriteLine(categories);
        Console.WriteLine($"Joke is - {joke}");
        host.Start();
    }
}

