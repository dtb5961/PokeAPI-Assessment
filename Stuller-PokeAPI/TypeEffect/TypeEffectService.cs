using System.Net;
using System.Net.WebSockets;

public class TypeEffectService : ITypeEffectService
{
    private readonly HttpClient _httpClient;

    public TypeEffectService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PokeApi");
    }

    public HttpClient HttpClient { get; }

    public async Task<TypeEffectInfo> FetchTypeEffectInfoAsync(String pokemonType)
    {

        var response = await _httpClient.GetAsync($"type/{pokemonType}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        TypeEffectInfo pokemonTypeEffectInfo = await response.Content.ReadAsAsync<TypeEffectInfo>().ConfigureAwait(false);


        return pokemonTypeEffectInfo;
    }

}


