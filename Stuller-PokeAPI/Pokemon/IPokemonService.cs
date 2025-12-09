public interface IPokemonService
{
    Task<PokeApiAttributeResult> GetPokemonAttributesAsync(String userPokemon);

    Task<PokeApiPokedexResult> GetPokedexAsync();

    Task<PokeApiTypeEffectResult> GetTypeEffectInfoAsync(String pokemonType);
}

