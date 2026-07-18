using Nycto.Domain;
using Discord;
using Nycto.Services;

namespace Nycto.UI;

public static class ItemView
{
    public static Embed CreateItemEmbed(ItemModel item, string locale = "en-US")
    {
        var embed = BaseItemView.CreateBaseEmbed(item);
        
        BaseItemView.AddDropLocations(embed, item);

        return embed.Build();
    }
}
