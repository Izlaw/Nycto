using Nycto.Domain;
using Discord;

namespace Nycto.UI;

public static class ModView
{
    public static Embed CreateModEmbed(ItemModel item, string locale = "en-US")
    {
        var embed = BaseItemView.CreateBaseEmbed(item);
        
        BaseItemView.AddDropLocations(embed, item);

        return embed.Build();
    }
}
