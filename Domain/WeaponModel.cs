using System.Text.Json.Serialization;

namespace Nycto.Domain;

public class WeaponModel : ItemModel
{
    [JsonPropertyName("totalDamage")] public float? TotalDamage { get; set; }
    [JsonPropertyName("criticalChance")] public float? CriticalChance { get; set; }
    [JsonPropertyName("criticalMultiplier")] public float? CriticalMultiplier { get; set; }
}
