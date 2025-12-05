// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

public class PokemonAttributes
{
    // public List<Ability> abilities { get; set; }
    // public int base_experience { get; set; }
    // public Cries cries { get; set; }
    // public List<Form> forms { get; set; }
    // public List<GameIndex> game_indices { get; set; }
    // public int height { get; set; }
    // public List<object> held_items { get; set; }
    public int id { get; set; }
    // public bool is_default { get; set; }
    // public string location_area_encounters { get; set; }
    // public List<Move> moves { get; set; }
    public string name { get; set; }
    // public int order { get; set; }
    // public List<PastAbility> past_abilities { get; set; }
    // public List<object> past_types { get; set; }
    // public Species species { get; set; }
    //public Sprites sprites { get; set; }
    // public List<PokemonStat> stats { get; set; }
    public List<PokemonType> types { get; set; }
    // public int weight { get; set; }
}


public class PokemonType
{
    public int slot { get; set; }
    public ApiResource type { get; set; }

    public String GetPokemonType()
    {
        return type.name;
    }
}







