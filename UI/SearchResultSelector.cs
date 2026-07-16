using Nycto.Domain;
using Discord;
using Nycto.Services;

namespace Nycto.UI;

public static class SearchResultSelector
{
    public static Embed CreateEmbed(ItemModel item)
    {
        var category = item.Category?.ToLowerInvariant();

        return category switch
        {
            "warframes" => WarframeView.CreateWarframeEmbed(item),
            "primary" or "secondary" or "melee" => WeaponView.CreateWeaponEmbed(item),
            "mods" => ModView.CreateModEmbed(item),
            _ => ItemView.CreateItemEmbed(item)
        };
    }
}
