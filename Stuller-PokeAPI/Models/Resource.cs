using Newtonsoft.Json;

[JsonObject]
public class Resource
{
    [JsonProperty("name")]
    public String resourceName{ get; set;  }

    public String url{ get; set; }
}