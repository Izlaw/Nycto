using Nycto.Domain;
using Discord;
using Discord.Interactions;
using Nycto.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Nycto.Controllers;

public class WarframeController : InteractionModuleBase<SocketInteractionContext>
{
    private readonly WarframeAPIService _apiService;

    public WarframeController(WarframeAPIService apiService)
    {
        _apiService = apiService;
    }

    [SlashCommand("wf", "Search for a Warframe item or mod on the wiki")]
    [IntegrationType(ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall)]
    [CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
    public async Task SearchItemAsync([Summary("search", "The name of the item or mod to search for"), Autocomplete(typeof(ItemAutocompleteHelper))] string searchQuery)
    {
        await DeferAsync();

        var item = await _apiService.SearchItemAsync(searchQuery);

        if (item == null)
        {
            await FollowupAsync($"Could not find any item matching `{searchQuery}`.", ephemeral: true);
            return;
        }

        var embed = Nycto.UI.SearchResultSelector.CreateEmbed(item);

        await FollowupAsync(embed: embed);
    }
}

public class ItemAutocompleteHelper : AutocompleteHandler
{
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(
        IInteractionContext context, 
        IAutocompleteInteraction autocompleteInteraction, 
        IParameterInfo parameter, 
        IServiceProvider services)
    {
        var apiService = services.GetRequiredService<WarframeAPIService>();
        
        var userInput = autocompleteInteraction.Data.Current.Value?.ToString() ?? string.Empty;
        var items = await apiService.AutocompleteSearchAsync(userInput);

        var results = items.Select(x => new AutocompleteResult(x.Name, x.Name)).ToList();

        return AutocompletionResult.FromSuccess(results);
    }
}
