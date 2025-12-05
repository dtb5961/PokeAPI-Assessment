public class AppRunner
{
    private readonly IPokemonService _pokemonService;
    private readonly ITypeEffectService _typeEffectService;
    private readonly IFuzzyMatcher _fuzzyMatcher;

    public AppRunner(
        IPokemonService pokemonService,
                                      ITypeEffectService typeEffectService,
                                                                          IFuzzyMatcher fuzzyMatcher)
    {
        _pokemonService = pokemonService;
        _typeEffectService = typeEffectService;
        _fuzzyMatcher = fuzzyMatcher;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Enter a Pokémon name:");
        var input = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No Pokémon Entered Exiting");
            return;
        }

        PokemonAttributes pokemon = await _pokemonService.FetchPokemonAttributesAsync(input);

        while (pokemon == null)
        {
            Console.WriteLine($"No Pokémon found for '{input}'.");

            List<String> pokedex = await _pokemonService.FetchPokedexAsync();

            //Optional fuzzy suggestions
            var suggestions = await _fuzzyMatcher.FindBestMatchAsync(input, pokedex);

            if (suggestions.Any())
            {
                Console.WriteLine("Did you mean:");

                var suggestionList = suggestions.ToList();

                for (int i = 0; i < suggestionList.Count; i++)
                {
                    Console.WriteLine($" {i + 1}. {suggestionList[i]}");
                }

                Console.WriteLine("Enter the number of the Pokémon you meant, or press Enter to cancel:");

                var selectionInput = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(selectionInput))
                {
                    // User chose not to pick any suggestion
                    Console.WriteLine("No selection made. Exiting.");
                    return;
                }

                if (int.TryParse(selectionInput, out int selectedIndex))
                {
                    // Convert to zero-based index
                    selectedIndex -= 1;

                    if (selectedIndex >= 0 && selectedIndex < suggestionList.Count)
                    {
                        var selectedName = suggestionList[selectedIndex];
                        Console.WriteLine($"Fetching data for '{selectedName}'...");

                        pokemon = await _pokemonService.FetchPokemonAttributesAsync(selectedName);
                    }
                }
            }

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
