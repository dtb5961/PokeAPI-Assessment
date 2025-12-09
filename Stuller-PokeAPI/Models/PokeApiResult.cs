public enum PokeApiStatus
{
    Success,
    NotFound,
    Error
}

public sealed class PokeApiAttributeResult
{
    public PokeApiStatus Status { get; }

    public PokemonAttributes? PokemonAttributesResponse { get; }

    public string? ErrorMessage { get; }

    private PokeApiAttributeResult(PokeApiStatus status, PokemonAttributes? pokemonAttributeResponse = null, string? errorMessage = null)
    {
        Status = status;
        PokemonAttributesResponse = pokemonAttributeResponse;
        ErrorMessage = errorMessage;
    }

    public static PokeApiAttributeResult Success(PokemonAttributes pokemon) =>
    new PokeApiAttributeResult(PokeApiStatus.Success, pokemon);

    public static PokeApiAttributeResult NotFound() =>
        new PokeApiAttributeResult(PokeApiStatus.NotFound);

    public static PokeApiAttributeResult Error(string message) =>
        new PokeApiAttributeResult(PokeApiStatus.Error, errorMessage: message);
}

public sealed class PokeApiTypeEffectResult
{
    public PokeApiStatus Status { get; }

    public TypeEffectInfo? TypeEffectInfoResponse { get; }

    public string? ErrorMessage { get; }

    private PokeApiTypeEffectResult(PokeApiStatus status, TypeEffectInfo? typeEffectInfoResponse = null, string? errorMessage = null)
    {
        Status = status;
        TypeEffectInfoResponse = typeEffectInfoResponse;
        ErrorMessage = errorMessage;
    }

    public static PokeApiTypeEffectResult Success(TypeEffectInfo typeEffectInfoResponse) =>
    new PokeApiTypeEffectResult(PokeApiStatus.Success, typeEffectInfoResponse);

    public static PokeApiTypeEffectResult Error(string message) =>
        new PokeApiTypeEffectResult(PokeApiStatus.Error, errorMessage: message);
}

public sealed class PokeApiPokedexResult
{
    public PokeApiStatus Status { get; }

    public List<String>? PokedexResponse { get; }

    public string? ErrorMessage { get; }

    private PokeApiPokedexResult(PokeApiStatus status, List<String>? pokedexResult = null, string? errorMessage = null)
    {
        Status = status;
        PokedexResponse = pokedexResult;
        ErrorMessage = errorMessage;
    }

    public static PokeApiPokedexResult Success(List<String> pokedexResult) =>
    new PokeApiPokedexResult(PokeApiStatus.Success, pokedexResult);

    public static PokeApiPokedexResult Error(string message) =>
        new PokeApiPokedexResult(PokeApiStatus.Error, errorMessage: message);
}