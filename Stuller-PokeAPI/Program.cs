using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PokemonConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.ClearProviders())
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
