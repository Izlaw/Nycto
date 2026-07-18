using Nycto.Domain;
using Discord;
using Nycto.Services;

namespace Nycto.UI;

public static class WarframeView
{
    public static Embed CreateWarframeEmbed(ItemModel baseItem, string locale = "en-US")
    {
        var embed = BaseItemView.CreateBaseEmbed(baseItem);

        if (baseItem is WarframeModel item)
        {
            if (item.Health.HasValue && item.Shield.HasValue && item.Armor.HasValue)
            {
                embed.AddField("📊 Base Stats", $"**Health:** {item.Health} | **Shield:** {item.Shield}\n**Armor:** {item.Armor} | **Energy:** {item.Power ?? 0}", false);
            }

            if (item.Abilities != null && item.Abilities.Any())
            {
                var abilitiesText = string.Join("\n", item.Abilities.Select(a => $"**{a.Name}:** {Nycto.Helper.EmojiMapper.ReplaceEmojiTags(a.Description)}"));
                if (abilitiesText.Length > 1024) abilitiesText = abilitiesText.Substring(0, 1021) + "...";
                embed.AddField("✨ Abilities", abilitiesText, false);
            }
        }

        BaseItemView.AddDropLocations(embed, baseItem);

        return embed.Build();
    }
}
