public interface IPokemonService
{
    Task<PokemonAttributes> FetchPokemonAttributesAsync(String userPokemon);
    Task<List<String>> FetchPokedexAsync();
}

