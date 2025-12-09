using FuzzySharp;

public static class Utility
{

    public static String GetPokemonTypeFromAttributes(PokemonAttributes pokemonAttributes)
    {
        List<PokemonType> pokemonTypeList = pokemonAttributes.types;
        if (pokemonTypeList.Count == 1)
        {
            return pokemonTypeList[0].GetPokemonType();
        }
        else
        {
            return string.Join(", ", pokemonTypeList.Select(pokemon => pokemon.GetPokemonType()));
        }

    }

    public static void ProcessDamageEffect(DamageRelations pokemonDamageInfo, List<String> strongAgainst, List<String> weakAgainst)
    {
        //Updating Strong List
        UpdateDamageList(strongAgainst, pokemonDamageInfo.double_damage_to);
        UpdateDamageList(strongAgainst, pokemonDamageInfo.no_damage_from);
        UpdateDamageList(strongAgainst, pokemonDamageInfo.half_damage_from);

        //Updating Weak List
        UpdateDamageList(weakAgainst, pokemonDamageInfo.no_damage_to);
        UpdateDamageList(weakAgainst, pokemonDamageInfo.half_damage_to);
        UpdateDamageList(weakAgainst, pokemonDamageInfo.double_damage_from);

    }

    public static List<String> FindBestMatches(String userPokemon, List<String> pokedex)
    {

        var bestMatch = Process.ExtractTop(userPokemon, pokedex);

        return bestMatch.ToList().Select(suggestion => suggestion.Value).ToList();
    }

    public static void CleanDamageList(List<String> damageList)
    {
        var cleanedList = damageList.Distinct().ToList();
        damageList.Clear();
        damageList.AddRange(cleanedList);
    }

    private static void UpdateDamageList(List<String> damageList, List<ApiResource> damageInfoList)
    {
        damageList.AddRange(damageInfoList.Select(damageType => damageType.name).ToList());

    }


}