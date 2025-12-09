public static class TestUtility
{

    public static String squirtleResponseFile = "squirtle-water_response.json";

    public static String wooperResponseFile = "wooper_water-ground.json";

    public static String pokedexResponseFile = "pokedex_response.json";

    public static String waterTypeEffectResponseFile = "water-type_response.json";

    public static String groundTypeEffectResponseFile = "ground-type_response.json";

    public static String LoadFileFromJson(String fileName)
    {

        var basePath = AppContext.BaseDirectory;
        var filePath = Path.Combine(basePath, "Resources", fileName);
        return File.ReadAllText(filePath);
    }
}