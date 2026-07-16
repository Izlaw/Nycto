using Nycto.Domain;
using Discord;
using Nycto.Services;

namespace Nycto.UI;

public static class WeaponView
{
    public static Embed CreateWeaponEmbed(ItemModel item)
    {
        var embed = BaseItemView.CreateBaseEmbed(item);

        var weaponStats = new Dictionary<string, string>
        {
            { "totalDamage", "💥 Damage" },
            { "criticalChance", "🎯 Crit Chance" },
            { "criticalMultiplier", "✖️ Crit Multiplier" },
            { "statusChance", "🧪 Status" },
            { "fireRate", "🔫 Fire Rate" },
            { "magazineSize", "📦 Magazine" },
            { "reloadTime", "⏱️ Reload Time" }
        };

        BaseItemView.AddDynamicStats(embed, item, "💥 Combat Stats", weaponStats);
        BaseItemView.AddDamageTypes(embed, item);

        BaseItemView.AddDropLocations(embed, item);

        return embed.Build();
    }
}
