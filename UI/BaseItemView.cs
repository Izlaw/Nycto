using Nycto.Domain;
using Discord;
using Nycto.Services;
using Nycto.Helper;

namespace Nycto.UI;

public abstract class BaseItemView
{
    public static EmbedBuilder CreateBaseEmbed(ItemModel item)
    {
        Color embedColor = item.Rarity?.ToLower() switch
        {
            "legendary" => new Color(255, 215, 0),
            "rare" => new Color(212, 175, 55),
            "uncommon" => new Color(192, 192, 192),
            "common" => new Color(205, 127, 50),
            _ => Color.DarkPurple
        };

        string description = item.Description;
        if (string.IsNullOrEmpty(description) && item.LevelStats != null && item.LevelStats.Any())
        {
            var listBuilder = new System.Text.StringBuilder();
            
            for (int i = 0; i < item.LevelStats.Count; i++)
            {
                var stats = item.LevelStats[i].Stats;
                if (stats != null && stats.Any())
                {
                    string joinedStats = string.Join(" | ", stats);
                    listBuilder.AppendLine($"**Rank {i}:** {joinedStats}");
                }
            }
            
            description = EmojiMapper.ReplaceEmojiTags(listBuilder.ToString());
        }

        var embed = new EmbedBuilder()
            .WithTitle($"**{item.Name}**")
            .WithUrl(item.WikiaUrl)
            .WithDescription(description)
            .WithColor(embedColor)
            .WithThumbnailUrl(item.GetImageUrl())
            .WithFooter("Data provided by warframestat.us");

        embed.AddField("Category", string.IsNullOrEmpty(item.Category) ? "N/A" : item.Category, true);
        // embed.AddField("Type", string.IsNullOrEmpty(item.Type) ? "N/A" : item.Type, true);

        if (!string.IsNullOrEmpty(item.Polarity)) embed.AddField("💠 Polarity", Nycto.Helper.EmojiMapper.GetPolarityString(item.Polarity), true);
        if (!string.IsNullOrEmpty(item.Rarity)) embed.AddField("✨ Rarity", item.Rarity, true);

        return embed;
    }

    public static void AddDropLocations(EmbedBuilder embed, ItemModel item)
    {
        var allDrops = new List<ItemModel.Drop>();
        if (item.Drops != null) allDrops.AddRange(item.Drops);
        if (item.Components != null)
        {
            foreach (var comp in item.Components)
            {
                if (comp.Drops != null)
                {
                    foreach (var drop in comp.Drops)
                    {
                        drop.Location = $"**{comp.Name}**: {drop.Location}";
                        allDrops.Add(drop);
                    }
                }
            }
        }

        if (allDrops.Any())
        {
            var topDrops = allDrops
                .Where(d => d.Chance.HasValue)
                .OrderByDescending(d => d.Chance ?? 0f)
                .Take(3)
                .Select(d => $"📍 {d.Location} ({d.Chance:F2}%)")
                .ToList();
                
            if (topDrops.Any())
            {
                embed.AddField("🗺️ Top Drop Locations", string.Join("\n", topDrops), false);
            }
        }
    }

    public static void AddDynamicStats(EmbedBuilder embed, ItemModel item, string sectionTitle, Dictionary<string, string> statMapping)
    {
        if (item.AdditionalData == null) return;

        var textBlocks = new List<string>();

        foreach (var stat in statMapping)
        {
            var jsonKey = stat.Key;
            var displayName = stat.Value;

            if (item.AdditionalData.TryGetValue(jsonKey, out var element))
            {
                if (element.ValueKind != System.Text.Json.JsonValueKind.Null && element.ValueKind != System.Text.Json.JsonValueKind.Undefined)
                {
                    string valStr = FormatStatValue(jsonKey, element);
                    textBlocks.Add($"**{displayName}:** {valStr}");
                }
            }
        }

        if (textBlocks.Any())
        {
            embed.AddField(sectionTitle, string.Join("\n", textBlocks), false);
        }
    }

    public static void AddDamageTypes(EmbedBuilder embed, ItemModel item)
    {
        if (item.AdditionalData == null) return;

        if (item.AdditionalData.TryGetValue("damage", out var damageTypesElement))
        {
            if (damageTypesElement.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                var damageBlocks = new List<string>();

                double totalDamage = 0;
                foreach (var prop in damageTypesElement.EnumerateObject())
                {
                    if (prop.Name.Equals("total", StringComparison.OrdinalIgnoreCase)) continue;
                    if (prop.Value.TryGetDouble(out double v)) totalDamage += v;
                }

                foreach (var prop in damageTypesElement.EnumerateObject())
                {
                    if (prop.Name.Equals("total", StringComparison.OrdinalIgnoreCase)) continue;

                    if (prop.Value.TryGetDouble(out double dmgValue) && dmgValue > 0)
                    {
                        string fullString = EmojiMapper.GetElementString(prop.Name);
                        string emojiOnly = fullString.Split(' ')[0]; 
                        
                        double percentage = totalDamage > 0 ? (dmgValue / totalDamage) * 100 : 0;
                        damageBlocks.Add($"{emojiOnly} {dmgValue} ({percentage:F2}%)");
                    }
                }

                if (damageBlocks.Any())
                {
                    embed.AddField("Elemental Damage", string.Join(" | ", damageBlocks), false);
                }
            }
        }
    }

    private static string FormatStatValue(string jsonKey, System.Text.Json.JsonElement element)
    {
        string valStr = element.ToString();

        if (jsonKey == "criticalChance" || jsonKey == "procChance")
        {
            if (element.TryGetDouble(out double d)) return $"{d * 100:F2}%";
        }
        else if (jsonKey == "criticalMultiplier") return $"{valStr}x";
        else if (jsonKey == "reloadTime") return $"{valStr} s";
        else if (jsonKey == "accuracy")
        {
            if (element.TryGetDouble(out double acc))
            {
                string desc = "Very Low";
                if (acc >= 100) desc = "Very High";
                else if (acc >= 50) desc = "High";
                else if (acc >= 16) desc = "Medium";
                else if (acc >= 9) desc = "Low";
                return $"{desc} ({valStr})";
            }
        }
        else if (jsonKey == "exilusPolarity")
        {
            return EmojiMapper.GetPolarityString(valStr);
        }

        return valStr;
    }
}
