public class AppRunner
{
    private readonly IPokemonService _pokemonService;

    public AppRunner(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            List<String> pokedex = new List<String>();

            Console.WriteLine("Enter a Pokémon name (or press Enter to exit):");
            var input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No Pokémon Entered Exiting");
                return;
            }

            PokeApiAttributeResult pokemonResponse = await _pokemonService.GetPokemonAttributesAsync(input);

            while (pokemonResponse.Status.Equals(PokeApiStatus.NotFound))
            {
                Console.WriteLine($"No Pokémon found for '{input}'.");

                if (pokedex == null || pokedex.Count == 0)
                {
                    PokeApiPokedexResult pokedexResponse = await _pokemonService.GetPokedexAsync();
                    if (pokedexResponse.Status.Equals(PokeApiStatus.Success))
                    {
                        pokedex = pokedexResponse.PokedexResponse;
                    }
                    else
                    {
                        Console.WriteLine(pokemonResponse.ErrorMessage);
                        return;
                    }
                }

                List<String> suggestions = Utility.FindBestMatches(input, pokedex);

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

                            pokemonResponse = await _pokemonService.GetPokemonAttributesAsync(selectedName);
                        }
                    }
                }

                if (pokemonResponse.Status.Equals(PokeApiStatus.Error))
                {
                    Console.WriteLine(pokemonResponse.ErrorMessage);
                    return;
                }

            }

            String pokemonTypes = Utility.GetPokemonTypeFromAttributes(pokemonResponse.PokemonAttributesResponse);
            Console.WriteLine($"\nPokemon is {pokemonResponse.PokemonAttributesResponse.name} and type/s is/are: {pokemonTypes}\n");

            List<String> pokemonTypesList = pokemonResponse.PokemonAttributesResponse.types.Select(pokemonType => pokemonType.GetPokemonType()).ToList();

            List<String> strongAgainst = new List<String>();
            List<String> weakAgainst = new List<String>();


            var fetchDamageResponses = pokemonTypesList.Select(pokemonType => _pokemonService.GetTypeEffectInfoAsync(pokemonType)).ToList();

            var damageResponses = await Task.WhenAll(fetchDamageResponses);

            var errorDamageResponse = damageResponses.ToList().Find(damageResponse => damageResponse.Status.Equals(PokeApiStatus.Error));
            if (errorDamageResponse != null)
            {
                Console.WriteLine(errorDamageResponse.ErrorMessage);
                return;
            }

            damageResponses.ToList().ForEach(damageResponse => Utility.ProcessDamageEffect(damageResponse.TypeEffectInfoResponse.damage_relations, strongAgainst, weakAgainst));

            Utility.CleanDamageList(strongAgainst);
            Utility.CleanDamageList(weakAgainst);

            Console.WriteLine("Strong against: " + string.Join(", ", strongAgainst));
            Console.WriteLine("Weak against: " + string.Join(", ", weakAgainst));
            Console.WriteLine();
        }
    }
}
