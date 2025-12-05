

using FuzzySharp;

public class FuzzyMatcher : IFuzzyMatcher
{


    public async Task<List<String>> FindBestMatchAsync(String userPokemon, List<String> pokedex)
    {

        var bestMatch = Process.ExtractTop(userPokemon, pokedex);

        return bestMatch.ToList().Select(suggestion => suggestion.Value).ToList();
    }

}