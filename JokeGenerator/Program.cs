﻿using JokeGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JokeGenerator.Models;
using Microsoft.Extensions.Http;

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

        #region Configure DI container
        builder.Services.AddTransient<IJokeProvider,JokeProvider>();
        builder.Services.AddHttpClient(AppConstants.CNJokeGenerator, client => {
            var baseAddress = builder.Configuration.GetSection("JokesAPI:EndPoint").Value;
            client.BaseAddress = new Uri(baseAddress);
        });
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
