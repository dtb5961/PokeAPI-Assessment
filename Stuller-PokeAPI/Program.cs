using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register a named HttpClient for PokeAPI
                    services.AddHttpClient("PokeApi", client =>
                    {
                        client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                        client.Timeout = TimeSpan.FromSeconds(10);
                    });

                    // Register application services
                    services.AddTransient<IPokemonService, PokemonService>();
                    services.AddTransient<ITypeEffectService, TypeEffectService>();

                    // Optional bonus: fuzzy search service
                    services.AddTransient<IFuzzyMatcher, FuzzyMatcher>();

                    // Register console runner
                    services.AddTransient<AppRunner>();
                })
                .Build();

            // Run the main application loop
            var app = host.Services.GetRequiredService<AppRunner>();
            await app.RunAsync();
        }
    }
}
