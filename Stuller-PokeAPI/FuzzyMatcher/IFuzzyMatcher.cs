public interface IFuzzyMatcher
{
    Task<List<String>> FindBestMatchAsync(String userPokemon, List<String> pokedex);
}