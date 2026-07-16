using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nycto.Services;

public class DiscordCommandService : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _commands;
    private readonly IServiceProvider _services;
    private readonly ILogger<DiscordCommandService> _logger;

    public DiscordCommandService(
        DiscordSocketClient client,
        InteractionService commands,
        IServiceProvider services,
        ILogger<DiscordCommandService> logger)
    {
        _client = client;
        _commands = commands;
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Ready += ReadyAsync;
        _commands.Log += LogAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        _client.InteractionCreated += HandleInteraction;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Ready -= ReadyAsync;
        _commands.Log -= LogAsync;
        _client.InteractionCreated -= HandleInteraction;
        return Task.CompletedTask;
    }

    private async Task ReadyAsync()
    {
        _logger.LogInformation("Bot is ready. Registering commands...");
        
        try
        {
            await _commands.RegisterCommandsGloballyAsync();
            _logger.LogInformation("Commands registered globally.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering commands");
        }
    }

    private Task LogAsync(LogMessage log)
    {
        _logger.LogInformation("InteractionService: {Message}", log.Message);
        return Task.CompletedTask;
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(_client, interaction);
            
            var result = await _commands.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
            {
                _logger.LogError("Error executing interaction: {ErrorReason}", result.ErrorReason);
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        break;
                    default:
                        break;
                }
            }
        }
        catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
