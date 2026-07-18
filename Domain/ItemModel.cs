namespace Nycto.Domain;

public class ItemModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("wikiaUrl")]
    public string WikiaUrl { get; set; } = string.Empty;

    [JsonPropertyName("imageName")]
    public string ImageName { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("polarity")]
    public string Polarity { get; set; } = string.Empty;

    [JsonPropertyName("rarity")]
    public string Rarity { get; set; } = string.Empty;

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }

    [JsonPropertyName("components")] public List<Component>? Components { get; set; }
    [JsonPropertyName("drops")] public List<Drop>? Drops { get; set; }
    [JsonPropertyName("levelStats")] public List<LevelStat>? LevelStats { get; set; }
    [JsonPropertyName("baseDrain")] public int? BaseDrain { get; set; }
    [JsonPropertyName("fusionLimit")] public int? FusionLimit { get; set; }
    
    public string GetImageUrl()
    {
        if (string.IsNullOrEmpty(ImageName))
            return string.Empty;
        return $"https://cdn.warframestat.us/img/{ImageName}";
    }

    public class Component
    {
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("drops")] public List<Drop>? Drops { get; set; }
    }

    public class Drop
    {
        [JsonPropertyName("location")] public string Location { get; set; } = string.Empty;
        [JsonPropertyName("chance")] public float? Chance { get; set; }
        [JsonPropertyName("rarity")] public string Rarity { get; set; } = string.Empty;
    }

    public class LevelStat
    {
        [JsonPropertyName("stats")] public List<string>? Stats { get; set; }
    }
}
