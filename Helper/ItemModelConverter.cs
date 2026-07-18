namespace Nycto.Helper;

public class ItemModelConverter : JsonConverter<ItemModel>
{
    public override ItemModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        string? category = null;
        if (root.TryGetProperty("category", out var categoryElement))
        {
            category = categoryElement.GetString()?.ToLowerInvariant();
        }

        var fallbackOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        ItemModel? item = category switch
        {
            "warframes" => JsonSerializer.Deserialize<WarframeModel>(root.GetRawText(), fallbackOptions),
            "primary" or "secondary" or "melee" => JsonSerializer.Deserialize<WeaponModel>(root.GetRawText(), fallbackOptions),
            _ => JsonSerializer.Deserialize<ItemModel>(root.GetRawText(), fallbackOptions)
        };

        return item;
    }

    public override void Write(Utf8JsonWriter writer, ItemModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
