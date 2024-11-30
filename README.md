# Csteam-works Integration Library

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com)


## Introduction

This project provides a C# library for interacting with the Steam Web API, enabling easy retrieval of basic Steam information such as games, players, and related data.  It leverages caching for improved performance.

## Features

The library offers the following services:

### SteamApp

* **`GetSteamAppIdData(string searchKeyword)`:** Retrieves a list of Steam applications based on a search keyword.  Results are cached for 6 hours.
* **`GetSteamAppDetails(int appid, string currency)`:** Fetches detailed information about a specific Steam application using its App ID.  Allows specifying an optional currency parameter.
* **`GetCurrentPlayerCount(int appId)`:** Retrieves the current number of players for a specified Steam application.

### SteamUser

* **`GetUserID(string key, string username)`:** Resolves a Steam username to a Steam User ID using a provided API key.
* **`GetPlayerSummaries(string key, string steamId)`:** Retrieves detailed summaries for a Steam player using their Steam ID.
* **`GetRecentlyPlayedGames(string key, string steamId)`:**  Retrieves a list of games recently played by a specified user.
* **`GetOwnedGames(string key, string steamId)`:** Fetches a list of games owned by a specified user.


## Installation

### Requirements

* .NET 9
* Your location access to Steam is not blocked
* A Steam Web API key (you can get one from [here](https://steamcommunity.com/dev/apikey))

### Installation Steps

1. **Install via NuGet (Recommended) (I'm working on it):**  This is the easiest way to install the library.  You can add it to your project directly through your IDE's NuGet package manager. (The package name will be `Csteam-works` -  you'll need to publish this to NuGet first if you want to use this method.)

2. **Manual Installation (Alternative):**
   ```bash
   git clone https://github.com/yourusername/Csteam-works.git
   cd Csteam-works
   dotnet restore
   ```
   Then, add the project as a reference to your own project.

## Example Usage

### 1. Retrieve Steam App Information

```csharp
using Csteam-works.Services;

var steamAppInfo = new SteamApp();
var apps = await steamAppInfo.GetSteamAppIdData("searchKeyword");
foreach (var app in apps)
{
    Console.WriteLine($"App ID: {app.appid}, Name: {app.name}");
}
```

### 2. Retrieve Steam User Information

```csharp
using Csteam-works.Services;

var steamUserInfo = new SteamUser();
var userId = await steamUserInfo.GetUserID("YOUR_API_KEY", "username");
if (userId != null)
{
    var playerSummaries = await steamUserInfo.GetPlayerSummaries("YOUR_API_KEY", userId.steamid);
    Console.WriteLine($"Player Name: {playerSummaries.response.players[0].personaname}");
}
```
You can see more function at **SteamUser** and **SteamApp** class

Remember to replace `"YOUR_API_KEY"` with your actual Steam Web API key. Similar to the others method that needed


## Directory Structure

```
Csteam-works/
├── Models/
│   ├── App/
│   ├── Player/
│   ├── Response/
└── Services/
    ├── SteamAppInformation.cs
    └── SteamUser.cs
```

## Contributing

I always welcome any contributions :D. Feel free to get involved!

## License

[MIT License](https://github.com/nupniichan/Csteam-works/blob/main/LICENSE)

## References
[.NET](https://github.com/dotnet/runtime)
[Microsoft.Extensions.Caching.Memory](https://www.nuget.org/packages/microsoft.extensions.caching.memory/)
[Steam Web Api](https://partner.steamgames.com/doc/webapi_overview)

## Contact

For any questions or issues, please open an issue on this GitHub repository.
