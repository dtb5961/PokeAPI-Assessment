public class AppRunner
{
    private readonly IPokemonService _pokemonService;
    private readonly ITypeEffectService _typeEffectService;
    //private readonly IFuzzyMatcher _fuzzyMatcher;

    public AppRunner(
        IPokemonService pokemonService,
                                      ITypeEffectService typeEffectService//,
                                                                          //IFuzzyMatcher fuzzyMatcher)
    )
    {
        _pokemonService = pokemonService;
        _typeEffectService = typeEffectService;
        //_fuzzyMatcher = fuzzyMatcher;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Enter a Pokémon name:");
        var input = Console.ReadLine()?.Trim() ?? "";

        PokemonAttributes pokemon = await _pokemonService.FetchPokemonAttributesAsync("Pikachu");

        if (pokemon == null)
        {
            Console.WriteLine($"No Pokémon found for '{input}'.");

            // Optional fuzzy suggestions
            //var suggestions = await _fuzzyMatcher.GetSuggestionsAsync(input);

            // if (suggestions.Any())
            // {
            //     Console.WriteLine("Did you mean:");
            //     foreach (var suggestion in suggestions)
            //     {
            //         Console.WriteLine($" - {suggestion}");
            //     }
            // }

            return;
        }
        String pokemonTypes = Utility.GetPokemonTypeFromAttributes(pokemon);
        Console.WriteLine($"Pokemon is {pokemon.name} and types are {pokemonTypes}");

        List<String> pokemonTypesList = pokemon.types.Select(pokemonType => pokemonType.GetPokemonType()).ToList();

        List<String> strongAgainst = new List<String>();
        List<String> weakAgainst = new List<String>();


        var fetchDamageResponses = pokemonTypesList.Select(pokemonType => _typeEffectService.FetchTypeEffectInfoAsync(pokemonType)).ToList();

        var damageResponses = await Task.WhenAll(fetchDamageResponses);

        damageResponses.ToList().ForEach(damageResponse => Utility.ProcessDamageEffect(damageResponse.damage_relations, strongAgainst, weakAgainst));

        
        Console.WriteLine("Strong against: " + string.Join(", ", strongAgainst));
        Console.WriteLine("Weak against: " + string.Join(", ", weakAgainst));
    }
}
