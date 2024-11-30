# Csteam-works Integration Library

## Introduction
This project provides a way to interact with the Steam API, allowing you to retrieve some basic information such as games, players, and other related data.

## Features
The library includes the following services:

### 1. SteamApp
- **GetSteamAppIdData(string searchKeyword)**: Retrieves a list of Steam applications based on a search keyword. It caches the results for 6 hours to improve performance.
- **GetSteamAppDetails(int appid, string currency)**: Fetches detailed information about a specific Steam application using its app ID and optional currency parameter.
- **GetCurrentPlayerCount(int appId)**: Retrieves the current number of players for a specific Steam application.

### 2. SteamUser
- **GetUserID(string key, string username)**: Resolves a Steam username to a Steam user ID using the provided API key.
- **GetPlayerSummaries(string key, string steamId)**: Fetches detailed summaries of a Steam player using their Steam ID.
- **GetRecentlyPlayedGames(string key, string steamId)**: Retrieves a list of games that the specified user has played recently.
- **GetOwnedGames(string key, string steamId)**: Fetches a list of games owned by the specified user.

## Installation

### Requirements
- .NET 5.0 or higher (.NET 9 is better)
- Your location is not blocked to access steam
- Steam key (you can get it on [here](https://steamcommunity.com/dev/apikey))

### Installing the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Csteam-works.git
   cd Csteam-works
   ```

2. Install the necessary packages:
   ```bash
   dotnet restore
   ```

## Usage

### 1. Retrieve Steam App Information
Use the `SteamAppInformation` class to fetch information about Steam applications.

#### Example:
```csharp
using steamapi.Services;

var steamAppInfo = new SteamAppInformation();
var apps = await steamAppInfo.GetSteamAppIdData("searchKeyword");
```

### 2. Retrieve Steam User Information
Use the `SteamUser` class to fetch information about Steam users.

#### Example:
```csharp
using steamapi.Services;

var steamUserInfo = new SteamUser();
var userId = await steamUserInfo.GetUserID("your_api_key", "username");
var playerSummaries = await steamUserInfo.GetPlayerSummaries("your_api_key", userId.steamid);
```

### 3. Retrieve Recently Played Games
```csharp
var recentlyPlayedGames = await steamUserInfo.GetRecentlyPlayedGames("your_api_key", userId.steamid);
```

### 4. Retrieve Owned Games
```csharp
var ownedGames = await steamUserInfo.GetOwnedGames("your_api_key", userId.steamid);
```

## Directory Structure
```
/steamapi
│
├── /Models
│   ├── /GameModels
│   ├── /PlayerModels
│   ├── /ResponseModels
│   └── /MiscModels
│
└── /Services
    ├── SteamAppInformation.cs
    └── SteamUser.cs
```

## Contributing
We welcome contributions to this project! Here’s how you can help:

1. **Fork the repository**: Click on the "Fork" button at the top right of the repository page.
2. **Clone your fork**: 
   ```bash
   git clone https://github.com/yourusername/Csteam-works.git
   cd Csteam-works
   ```
3. **Create a new branch**: 
   ```bash
   git checkout -b feature/YourFeatureName
   ```
4. **Make your changes**: Implement your feature or fix a bug.
5. **Commit your changes**: 
   ```bash
   git commit -m "Add your message here"
   ```
6. **Push to your fork**: 
   ```bash
   git push origin feature/YourFeatureName
   ```
7. **Create a pull request**: Go to the original repository and click on "New Pull Request".

## References
- [Steam Web API Documentation](https://developer.valvesoftware.com/wiki/Steam_Web_API)
- [Microsoft.Extensions.Caching.Memory](https://www.nuget.org/packages/microsoft.extensions.caching.memory/)

## Contact
If you have any questions, please contact me at: your.email@example.com.
