using Newtonsoft.Json;

namespace PokemonConsoleAppTest;

[TestClass]
public sealed class UtilityTests
{



    [TestMethod]
    public void TestGetPokemonTypeFromAttributes()
    {
        String squirtleJsonString = TestUtility.LoadFileFromJson(TestUtility.squirtleResponseFile);
        PokemonAttributes squirtleResponse = JsonConvert.DeserializeObject<PokemonAttributes>(squirtleJsonString);

        Assert.AreEqual(squirtleResponse.name, "squirtle");

        String squirtleType = Utility.GetPokemonTypeFromAttributes(squirtleResponse);

        Assert.AreEqual(squirtleType, "water");
    }

    [TestMethod]
    public void TestGetPokemonTypeFromAttributesMultipleTypes()
    {
        String wooperJsonString = TestUtility.LoadFileFromJson(TestUtility.wooperResponseFile);
        PokemonAttributes wooperResponse = JsonConvert.DeserializeObject<PokemonAttributes>(wooperJsonString);

        Assert.AreEqual(wooperResponse.name, "wooper");

        String wooperTypes = Utility.GetPokemonTypeFromAttributes(wooperResponse);
        Assert.Contains("water", wooperTypes);
        Assert.Contains("ground", wooperTypes);


    }

    [TestMethod]
    public void TestFindBestMatches()
    {
        String pokedexJsonString = TestUtility.LoadFileFromJson(TestUtility.pokedexResponseFile);

        Pokedex pokedexResponse = JsonConvert.DeserializeObject<Pokedex>(pokedexJsonString);

        Assert.AreEqual(1328, pokedexResponse.count);

        List<String> pokedexList = pokedexResponse.results.Select(pokemon => pokemon.name).ToList();

        List<String> suggestions = Utility.FindBestMatches("Pik", pokedexList);

        Assert.Contains("pikachu", suggestions);

    }

    [TestMethod]
    public void TestProcessDamageEffect()
    {
        String waterTypeEffectJsonString = TestUtility.LoadFileFromJson(TestUtility.waterTypeEffectResponseFile);

        TypeEffectInfo waterTypeEffectResponse = JsonConvert.DeserializeObject<TypeEffectInfo>(waterTypeEffectJsonString);

        Assert.AreEqual("water", waterTypeEffectResponse.name);

        List<String> strongAgainst = new List<String>();
        List<String> weakAgainst = new List<String>();

        Utility.ProcessDamageEffect(waterTypeEffectResponse.damage_relations, strongAgainst, weakAgainst);

        Assert.AreEqual(7, strongAgainst.Count);
        Assert.AreEqual(5, weakAgainst.Count);




    }


}
