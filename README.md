# Warframe Discord App

A Discord "User App" built with C# and .NET 9 that allows users to fetch Warframe item/mod information directly from the Warframe Wiki (via the WarframeStat.us API). 

Since this is built as a **User App**, you can install it directly to your Discord account and use it in *any* server or Direct Message by typing `/wf [item name]`, without needing to invite a bot to a specific server.

## Tech Stack
- **Language**: C# (.NET 9)
- **Discord API Wrapper**: [Discord.Net](https://github.com/discord-net/Discord.Net)
- **Framework**: `Microsoft.Extensions.Hosting` (Worker Service style)
- **Data Source**: [WarframeStat.us API](https://docs.warframestat.us/)

## Setup Instructions

### 1. Create the Application in Discord
1. Go to the [Discord Developer Portal](https://discord.com/developers/applications).
2. Click **New Application** and give it a name (e.g., "Warframe Helper").

### 2. Configure as a User App
1. On the left sidebar, click **Installation**.
2. Under **Installation Contexts**, check the box for **User Install**.
3. *Optional*: Uncheck **Guild Install** if you only want it to be a User App.
4. Under **Default Install Settings**, select `User Install` and ensure the **Scopes** include `applications.commands`.
5. Click **Save Changes**.

### 3. Get your Bot Token
1. On the left sidebar, click **Bot**.
2. Click **Reset Token** to generate your bot's token. 
3. **Copy this token**. *(Never commit this token to version control!)*

### 4. Configure the Code
1. Open the file `appsettings.json` in this project folder.
2. Replace `"YOUR_DISCORD_BOT_TOKEN_HERE"` with the token you copied in Step 3.

### 5. Build and Run
In your terminal, run the following commands:
```bash
dotnet restore
dotnet build
dotnet run
```
You should see console logs confirming that the bot has connected and registered its commands globally.

> **Note:** Global slash command registration can take up to an hour to propagate in Discord the first time you run it, but it is usually much faster for user apps.

### 6. Install the App to your Account
1. Go back to the **Installation** tab in the Developer Portal.
2. Under the **User Install** section, you will see an **Install Link**.
3. Open that link in your browser and authorize the app to your account.

### 7. Usage
You can now go to *any* DM or server and type:
`/wf item: Primed Flow`

The bot will reply with a rich embed containing the item's description, thumbnail, and wiki URL!

## Project Structure
- `Program.cs`: Sets up the dependency injection and hosting environment.
- `Services/BotService.cs`: Manages the Discord client connection lifecycle (login, start, stop).
- `Services/InteractionHandler.cs`: Listens for incoming slash commands and routes them to the correct module.
- `Services/WarframeApiService.cs`: Handles HTTP requests to the `api.warframestat.us` API to fetch item data.
- `Modules/WarframeModule.cs`: Contains the actual `/wf` slash command logic and builds the Discord UI embed.
