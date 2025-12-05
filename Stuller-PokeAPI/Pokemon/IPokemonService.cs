public interface IPokemonService
{
    Task<PokemonAttributes> FetchPokemonAttributesAsync(String userPokemon);
}

