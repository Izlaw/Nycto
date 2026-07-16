using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Nycto.Domain;

namespace Nycto.Services;

public class WarframeAPIService
{
    private readonly HttpClient _httpClient;

    public WarframeAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.warframestat.us/");
    }

    public async Task<ItemModel?> SearchItemAsync(string query)
    {
        var response = await _httpClient.GetAsync($"items/search/{Uri.EscapeDataString(query.ToLowerInvariant())}");
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new Nycto.Helper.ItemModelConverter());

        var items = await response.Content.ReadFromJsonAsync<ItemModel[]>(options);
        
        if (items == null) return null;

        var exactMatch = items.FirstOrDefault(i => i.Name.Equals(query, StringComparison.OrdinalIgnoreCase));
        if (exactMatch != null) return exactMatch;

        return items.FirstOrDefault(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<ItemModel>> AutocompleteSearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new ItemModel[]
            {
                new WarframeModel { Name = "Excalibur" },
                new WarframeModel { Name = "Rhino" },
                new WarframeModel { Name = "Volt" },
                new WarframeModel { Name = "Mag" },
                new WarframeModel { Name = "Wisp" }
            };
        }

        if (query.Length < 2) 
        {
            return Enumerable.Empty<ItemModel>();
        }

        var response = await _httpClient.GetAsync($"items/search/{Uri.EscapeDataString(query.ToLowerInvariant())}");
        if (!response.IsSuccessStatusCode)
        {
            return Enumerable.Empty<ItemModel>();
        }

        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new Nycto.Helper.ItemModelConverter());

        var items = await response.Content.ReadFromJsonAsync<ItemModel[]>(options);
        
        if (items == null) return Enumerable.Empty<ItemModel>();

        return items.Where(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(i => i.Name.Equals(query, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                    .ThenBy(i => IsHighPriorityCategory(i.Category) ? 0 : 1)
                    .ThenBy(i => i.Name.Length)
                    .Take(25);
    }

    private bool IsHighPriorityCategory(string category)
    {
        if (string.IsNullOrEmpty(category)) return false;
        var lower = category.ToLower();
        return lower == "warframes" || lower == "primary" || lower == "secondary" || lower == "melee" || lower == "mods" || lower == "archwing";
    }
}

