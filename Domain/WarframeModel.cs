namespace Nycto.Domain;

public class WarframeModel : ItemModel
{
    [JsonPropertyName("health")] public int? Health { get; set; }
    [JsonPropertyName("shield")] public int? Shield { get; set; }
    [JsonPropertyName("armor")] public int? Armor { get; set; }
    [JsonPropertyName("power")] public int? Power { get; set; }

    [JsonPropertyName("abilities")] public List<Ability>? Abilities { get; set; }

    public class Ability
    {
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
    }
}
