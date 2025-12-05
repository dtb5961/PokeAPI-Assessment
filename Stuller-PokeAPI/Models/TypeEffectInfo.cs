// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;


public class TypeEffectInfo
{
    public DamageRelations damage_relations { get; set; }
    public int id { get; set; }
    public string name { get; set; }
}

public class DamageRelations
{
    public List<ApiResource> double_damage_from { get; set; }
    public List<ApiResource> double_damage_to { get; set; }
    public List<ApiResource> half_damage_from { get; set; }
    public List<ApiResource> half_damage_to { get; set; }
    public List<ApiResource> no_damage_from { get; set; }
    public List<ApiResource> no_damage_to { get; set; }

}



public class Pokemon
{
    public Pokemon pokemon { get; set; }
    public int slot { get; set; }
    public string name { get; set; }
    public string url { get; set; }
}


