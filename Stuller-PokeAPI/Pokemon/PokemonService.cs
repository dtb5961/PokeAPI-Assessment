using System.Net;
using System.Net.WebSockets;

public class PokemonService : IPokemonService
{
    private readonly HttpClient _httpClient;

    public PokemonService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PokeApi");
    }

    public HttpClient HttpClient { get; }

    public async Task<PokemonAttributes> FetchPokemonAttributesAsync(String userPokemonName)
    {

        var response = await _httpClient.GetAsync($"pokemon/{userPokemonName}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        PokemonAttributes userPokemon = await response.Content.ReadAsAsync<PokemonAttributes>().ConfigureAwait(false);


        return userPokemon;
    }

}


