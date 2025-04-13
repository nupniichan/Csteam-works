# Csteam-works Integration Library

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-9.0-5C2D91?style=flat&logo=.net&logoColor=white)](https://dotnet.microsoft.com)
[![Steam](https://img.shields.io/badge/Steam-000000?style=flat&logo=steam&logoColor=white)](https://store.steampowered.com/)

## 📖 Introduction

Csteam-works is a robust C# library for interacting with the Steam Web API, enabling easy retrieval of Steam information including games, players, and related data. It leverages caching for improved performance and provides a clean, straightforward interface for developers.

## ✨ Features

The library offers the following services:

### 🎮 CsSteamApp

* **`GetSteamAppIdData(string searchKeyword)`:** Retrieves a list of Steam applications based on a search keyword.
* **`GetSteamAppDetails(int appid, string currency = "vn")`:** Fetches detailed information about a specific Steam application using its App ID, with options for currency specification.
* **`GetCurrentPlayerCount(int appId)`:** Retrieves the current number of players for a specified Steam application.
* **`GetSteamTopGames(int count = 100)`:** Gets a list of the top Steam games by player count, with a customizable number of results.

### 👤 CsSteamUser

* **`GetUserID(string key, string username)`:** Resolves a Steam username to a Steam User ID using a provided API key.
* **`GetPlayerSummaries(string key, string steamId)`:** Retrieves detailed summaries for a Steam player using their Steam ID.
* **`GetRecentlyPlayedGames(string key, string steamId)`:** Retrieves a list of games recently played by a specified user.
* **`GetOwnedGames(string key, string steamId)`:** Fetches a list of games owned by a specified user.
* **`GetUserStats(string key, string steamId)`:** Gets comprehensive statistics about a user, including total playtime, most played games, and more.
* **`GetPlayerBadges(string key, string steamId)`:** Retrieves badges earned by a player, including badge level, XP, and completion time.

## 🛠️ Installation

### Requirements

* .NET 9.0 or later
* Your location access to Steam is not blocked
* A Steam Web API key (get one from [Steam Developer Portal](https://steamcommunity.com/dev/apikey))

### Installation Steps

1. **Install via NuGet (Recommended) (Coming soon):** This is the easiest way to install the library. You can add it to your project directly through your IDE's NuGet package manager.

2. **Manual Installation (Alternative):**
   ```bash
   git clone https://github.com/nupniichan/Csteam-works.git
   cd Csteam-works
   dotnet restore
   ```
   Then, add the project as a reference to your own project.

## 🚀 Example Usage

### 1. Get Steam App Information

```csharp
using csteamworks.Services;

// Search for games by keyword
var steamAppInfo = new CsSteamApp();
var apps = await steamAppInfo.GetSteamAppIdData("Portal");
foreach (var app in apps)
{
    Console.WriteLine($"App ID: {app.appid}, Name: {app.name}");
}

// Get detailed information for a specific game
var gameDetails = await steamAppInfo.GetSteamAppDetails(400); // Portal
Console.WriteLine($"Game: {gameDetails.Data.Name}");
Console.WriteLine($"Players online: {gameDetails.Data.CurrentPlayerCount}");
```

### 2. Get Steam User Information

```csharp
using csteamworks.Services;

var steamUserInfo = new CsSteamUser();

// Convert username to Steam ID
var userId = await steamUserInfo.GetUserID("YOUR_API_KEY", "username");

// Get user profile information
var playerSummary = await steamUserInfo.GetPlayerSummaries("YOUR_API_KEY", userId.steamid);
Console.WriteLine($"Player: {playerSummary.personaname}");
Console.WriteLine($"Status: {playerSummary.OnlineStatus}");

// Get comprehensive user statistics
var userStats = await steamUserInfo.GetUserStats("YOUR_API_KEY", userId.steamid);
Console.WriteLine($"Total games owned: {userStats.TotalGamesOwned}");
Console.WriteLine($"Total playtime: {userStats.TotalPlaytimeHours} hours");
Console.WriteLine($"Library played: {userStats.PercentageOfLibraryPlayed}%");
```

Remember to replace `"YOUR_API_KEY"` with your actual Steam Web API key.

## 📁 Directory Structure

```
Csteam-works/
├── Models/
│   ├── App/                # Steam application models
│   │   ├── Components/     # Component models for applications
│   │   ├── Detail/         # Detailed app information models
│   │   ├── SteamApp.cs     # Basic Steam app model
│   │   ├── OwnedGame.cs    # User-owned game model
│   │   ├── Game.cs         # Game information model
│   │   └── ...
│   ├── User/               # Steam user-related models
│   │   ├── SteamUser.cs    # Basic user profile information
│   │   ├── SteamUserStats.cs # User statistics model
│   │   ├── SteamAppDetail.cs # Detailed app model for user context
│   │   └── ...
│   └── Response/           # API response models
│       ├── User/           # User API response models
│       └── App/            # App API response models
└── Services/
    ├── CsSteamApp.cs       # Services for Steam applications
    └── CsSteamUser.cs      # Services for Steam users
```

## 👨‍💻 Contributing

Contributions are always welcome! Whether it's bug fixes, feature enhancements, or documentation improvements, feel free to get involved!

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

[MIT License](https://github.com/nupniichan/Csteam-works/blob/main/LICENSE)

## 📚 References
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Microsoft.Extensions.Caching.Memory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.memory)
- [Steam Web API Documentation](https://partner.steamgames.com/doc/webapi_overview)
- [Steam Store API](https://wiki.teamfortress.com/wiki/User:RJackson/StorefrontAPI)

## 📞 Contact

For any questions or issues, please open an issue on this GitHub repository.
