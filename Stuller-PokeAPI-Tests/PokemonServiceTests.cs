

using System.Diagnostics;
using System.Net;
using FuzzySharp.Extractor;
using Moq;
using RichardSzalay.MockHttp;

namespace PokemonConsoleAppTest;

[TestClass]
public sealed class PokemonServiceTests
{

    public class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public FakeHttpClientFactory(HttpClient client)
        {
            _client = client;
        }

        public HttpClient CreateClient(string name)
        {

            return _client;
        }
    }


    [TestMethod]
    public async Task FetchPokemonAttributesAsync_ReturnsPokemon_OnSuccess()
    {
        string squirtleResponse = TestUtility.LoadFileFromJson(TestUtility.squirtleResponseFile); // file in Resources folder

        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("https://pokeapi.co/api/v2/pokemon/squirtle")
            .Respond("application/json", squirtleResponse);


        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");

        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(f => f.CreateClient("PokeApi"))
                   .Returns(httpClient);


        var service = new PokemonService(mockFactory.Object);

        PokeApiAttributeResult result = await service.GetPokemonAttributesAsync("squirtle");


        Assert.AreEqual("squirtle", result.PokemonAttributesResponse.name);
    }

    [TestMethod]
    public async Task FetchPokemonAttributesAsync_Returns404_NotFound()
    {
        string squirtleResponse = TestUtility.LoadFileFromJson(TestUtility.squirtleResponseFile); // file in Resources folder

        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("https://pokeapi.co/api/v2/pokemon/squir")
            .Respond(HttpStatusCode.NotFound);


        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");

        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(f => f.CreateClient("PokeApi"))
                   .Returns(httpClient);


        var service = new PokemonService(mockFactory.Object);

        PokeApiAttributeResult result = await service.GetPokemonAttributesAsync("squir");


        Assert.AreEqual(result.Status, PokeApiStatus.NotFound);
    }

    [TestMethod]
    public async Task FetchPokedexAsync()
    {
        string pokedexResponse = TestUtility.LoadFileFromJson(TestUtility.pokedexResponseFile); // file in Resources folder

        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("https://pokeapi.co/api/v2/pokemon?limit=-1")
            .Respond("application/json", pokedexResponse);


        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");

        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(f => f.CreateClient("PokeApi"))
                   .Returns(httpClient);


        var service = new PokemonService(mockFactory.Object);

        PokeApiPokedexResult result = await service.GetPokedexAsync();

        Assert.AreEqual(result.PokedexResponse.Count, 1327);
    }

    [TestMethod]
    public async Task FetchTypeEffectAsync()
    {
        string waterTypeResponse = TestUtility.LoadFileFromJson(TestUtility.waterTypeEffectResponseFile); // file in Resources folder

        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("https://pokeapi.co/api/v2/type/water")
            .Respond("application/json", waterTypeResponse);


        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");

        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(f => f.CreateClient("PokeApi"))
                   .Returns(httpClient);


        var service = new PokemonService(mockFactory.Object);

        PokeApiTypeEffectResult result = await service.GetTypeEffectInfoAsync("water");

        Assert.AreEqual("water", result.TypeEffectInfoResponse.name, ignoreCase: true);
    }
}