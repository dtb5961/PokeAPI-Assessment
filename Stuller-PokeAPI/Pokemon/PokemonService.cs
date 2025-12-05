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
        //response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        PokemonAttributes userPokemon = await response.Content.ReadAsAsync<PokemonAttributes>().ConfigureAwait(false);


        return userPokemon;
    }

    public async Task<List<String>> FetchPokedexAsync()
    {

        var response = await _httpClient.GetAsync("pokemon?limit=-1").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        Pokedex pokedex = await response.Content.ReadAsAsync<Pokedex>().ConfigureAwait(false);

        List<String> pokedexList = pokedex.results.Select(x => x.name).ToList();

        return pokedexList;


    }

    public async Task<TypeEffectInfo> FetchTypeEffectInfoAsync(String pokemonType)
    {

        var response = await _httpClient.GetAsync($"type/{pokemonType}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        TypeEffectInfo pokemonTypeEffectInfo = await response.Content.ReadAsAsync<TypeEffectInfo>().ConfigureAwait(false);


        return pokemonTypeEffectInfo;
    }

}


