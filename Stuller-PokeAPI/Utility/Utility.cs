public static class Utility
{

    public static String GetPokemonTypeFromAttributes(PokemonAttributes pokemonAttributes)
    {
        List<PokemonType> pokemonTypeList = pokemonAttributes.types;
        String parsedList = "";
        if (pokemonTypeList.Count == 1)
        {
            parsedList = pokemonTypeList[0].GetPokemonType();
        }
        else
        {
            pokemonTypeList.ForEach(pokemonType => parsedList += $"{pokemonType.GetPokemonType()}\t");
        }

        return parsedList;
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

    private static void UpdateDamageList(List<String> damageList, List<ApiResource> damageInfoList)
    {

        damageList.AddRange(damageInfoList.Select(damageType => damageType.name).ToList());

        var cleanedList = damageList.Distinct().ToList();
        damageList.Clear();
        damageList.AddRange(cleanedList);
    }
}