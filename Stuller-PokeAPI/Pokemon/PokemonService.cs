public class PokemonService : IPokemonService
{
    private readonly HttpClient _httpClient = null!;

    public PokemonService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PokeApi");
    }

    public HttpClient HttpClient { get; }

    public async Task<PokeApiAttributeResult> GetPokemonAttributesAsync(String userPokemonName)
    {

        var response = await _httpClient.GetAsync($"pokemon/{userPokemonName}").ConfigureAwait(false);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return PokeApiAttributeResult.NotFound();
        }

        else if (!response.IsSuccessStatusCode)
        {
            return PokeApiAttributeResult.Error($"Unexpected status code: {(int)response.StatusCode}");
        }


        PokemonAttributes userPokemon = await response.Content.ReadAsAsync<PokemonAttributes>().ConfigureAwait(false);

        return PokeApiAttributeResult.Success(userPokemon);
    }

    public async Task<PokeApiPokedexResult> GetPokedexAsync()
    {

        var response = await _httpClient.GetAsync("pokemon?limit=-1").ConfigureAwait(false);


        if (response.IsSuccessStatusCode)
        {
            Pokedex pokedex = await response.Content.ReadAsAsync<Pokedex>().ConfigureAwait(false);

            List<String> pokedexList = pokedex.results.Select(x => x.name).ToList();

            return PokeApiPokedexResult.Success(pokedexList);
        }
        else
        {
            return PokeApiPokedexResult.Error($"Unexpected status code: {(int)response.StatusCode}");
        }


    }

    public async Task<PokeApiTypeEffectResult> GetTypeEffectInfoAsync(String pokemonType)
    {

        var response = await _httpClient.GetAsync($"type/{pokemonType}").ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {

            TypeEffectInfo pokemonTypeEffectInfo = await response.Content.ReadAsAsync<TypeEffectInfo>().ConfigureAwait(false);

            return PokeApiTypeEffectResult.Success(pokemonTypeEffectInfo);
        }

        else
        {
            return PokeApiTypeEffectResult.Error($"Unexpected status code: {(int)response.StatusCode}");
        }
    }
}


