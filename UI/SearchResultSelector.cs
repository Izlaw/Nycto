using Nycto.Domain;
using Discord;
using Nycto.Services;

namespace Nycto.UI;

public static class SearchResultSelector
{
    public static Embed CreateEmbed(ItemModel item, string locale = "en-US")
    {
        var category = item.Category?.ToLowerInvariant();

        return category switch
        {
            "warframes" => WarframeView.CreateWarframeEmbed(item, locale),
            "primary" or "secondary" or "melee" => WeaponView.CreateWeaponEmbed(item, locale),
            "mods" => ModView.CreateModEmbed(item, locale),
            _ => ItemView.CreateItemEmbed(item, locale)
        };
    }
}
