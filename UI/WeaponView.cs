using Nycto.Domain;
using Discord;
using Nycto.Services;

namespace Nycto.UI;

public static class WeaponView
{
    public static Embed CreateWeaponEmbed(ItemModel item, string locale = "en-US")
    {
        var embed = BaseItemView.CreateBaseEmbed(item);

        var generalInfo = new Dictionary<string, string>
        {
            { "type", "Type" },
            { "masteryReq", "Mastery Rank Requirement" },
            { "trigger", "Trigger Type" }
        };
        BaseItemView.AddDynamicStats(embed, item, "General Information", generalInfo, locale);

        var utilityStats = new Dictionary<string, string>
        {
            { "accuracy", "Accuracy" },
            { "ammo", "Ammo Max" },
            { "ammoPickup", "Ammo Pickup" },
            { "disposition", "Disposition" },
            { "magazineSize", "Magazine Size" },
            { "reloadTime", "Reload Time" }
        };
        BaseItemView.AddDynamicStats(embed, item, "Utility", utilityStats, locale);

        var attackStats = new Dictionary<string, string>
        {
            { "totalDamage", "Total Damage" },
            { "criticalChance", "Crit Chance" },
            { "criticalMultiplier", "Crit Multiplier" },
            { "procChance", "Status Chance" },
            { "fireRate", "Fire Rate" },
            { "multishot", "Multishot" },
            { "noise", "Noise Level" },
            { "punchThrough", "Punch Through" },
            { "range", "Range" },
            { "spread", "Spread" },
            { "projectile", "Projectile Type" }
        };
        BaseItemView.AddDynamicStats(embed, item, "Normal Attack", attackStats, locale);

        var miscStats = new Dictionary<string, string>
        {
            { "exilusPolarity", "Exilus Polarity" },
            { "releaseDate", "Introduced" },
            { "buildPrice", "Build Price" }
        };
        BaseItemView.AddDynamicStats(embed, item, "Miscellaneous", miscStats, locale);

        BaseItemView.AddDamageTypes(embed, item, locale);

        BaseItemView.AddDropLocations(embed, item);

        return embed.Build();
    }
}
