using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nycto.Services;

namespace Nycto;

public class Program
{
    public static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.None,
                    LogLevel = LogSeverity.Info,
                });

                services.AddSingleton<DiscordSocketClient>();
                
                services.AddSingleton(new InteractionServiceConfig
                {
                    LogLevel = LogSeverity.Info,
                    UseCompiledLambda = true
                });

                services.AddSingleton(x => new InteractionService(
                    x.GetRequiredService<DiscordSocketClient>(),
                    x.GetRequiredService<InteractionServiceConfig>()
                ));
                
                services.AddHttpClient<WarframeAPIService>();
                
                services.AddHostedService<DiscordCommandService>();
                services.AddHostedService<BotService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

        await host.RunAsync();
    }
}
