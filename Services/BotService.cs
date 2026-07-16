using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nycto.Services;

public class BotService : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly IConfiguration _config;
    private readonly ILogger<BotService> _logger;

    public BotService(DiscordSocketClient client, IConfiguration config, ILogger<BotService> logger)
    {
        _client = client;
        _config = config;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Log += LogAsync;

        var token = _config["Discord:Token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogError("Discord bot token is missing in configuration! Please set 'Discord:Token' in appsettings.json.");
            return;
        }

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.StopAsync();
        await _client.LogoutAsync();
        _client.Log -= LogAsync;
    }

    private Task LogAsync(LogMessage log)
    {
        var logLevel = log.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };

        _logger.Log(logLevel, log.Exception, "Discord.Net: {Message}", log.Message);
        return Task.CompletedTask;
    }
}
