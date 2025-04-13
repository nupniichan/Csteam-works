using csteamworks.Models.App;
using csteamworks.Models.User;
using csteamworks.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace csteamworks.Services
{
    public class CsSteamUser
    {
        public async Task<SteamUserId> GetUserID(string key, string username)
        {
            string url = $"https://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={key}&vanityurl={username}";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<UserIdResponse>(json);

                if (result?.response?.success == 1)
                {
                    return result.response;
                }
                throw new Exception("Can't find user with that username.");
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting data: {e.Message}", e);
            }
        }

        public async Task<SteamUser> GetPlayerSummaries(string key, string steamId)
        {
            string url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamId}";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var playerSummariesResponse = JsonSerializer.Deserialize<UserSummariesResponse>(json);

                if (playerSummariesResponse?.response?.players?.Count > 0)
                {
                    return playerSummariesResponse.response.players[0];
                }
                throw new Exception("Can't find user with that steamid.");
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting data: {e.Message}", e);
            }
        }

        public async Task<List<Game>> GetRecentlyPlayedGames(string key, string steamId)
        {
            string url = $"https://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key={key}&steamid={steamId}&format=json";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var recentlyPlayedGamesResponse = JsonSerializer.Deserialize<RecentlyPlayedGamesResponse>(json);

                return recentlyPlayedGamesResponse?.response.games ?? new List<Game>();
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting data: {e.Message}", e);
            }
        }

        public async Task<List<OwnedGame>> GetOwnedGames(string key, string steamId)
        {
            string url = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={key}&steamid={steamId}&format=json&include_appinfo=true&include_played_free_games=true";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var ownedGamesResponse = JsonSerializer.Deserialize<OwnedGamesResponse>(json);
                var games = ownedGamesResponse?.response?.games ?? new List<OwnedGame>();
                
                foreach (var game in games)
                {
                    if (string.IsNullOrEmpty(game.name))
                    {
                        game.name = $"Game #{game.appid}";
                    }
                }

                return games;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting data: {e.Message}", e);
            }
        }
        
        public async Task<SteamUserStats> GetUserStats(string key, string steamId)
        {
            var userStats = new SteamUserStats
            {
                SteamId = steamId
            };
            
            try
            {
                var player = await GetPlayerSummaries(key, steamId);
                userStats.PlayerName = player.personaname;
                userStats.AvatarUrl = player.avatarfull;
                userStats.ProfileUrl = player.profileurl;
                userStats.AccountCreated = player.AccountCreatedDate;
                userStats.OnlineStatus = player.OnlineStatus;
                
                var ownedGames = await GetOwnedGames(key, steamId);
                userStats.TotalGamesOwned = ownedGames.Count;
                
                if (ownedGames.Count > 0)
                {
                    int totalMinutes = ownedGames.Sum(g => g.playtime_forever);
                    userStats.TotalPlaytimeHours = Math.Round(totalMinutes / 60.0, 1);
                    
                    userStats.MostPlayedGames = ownedGames
                        .OrderByDescending(g => g.playtime_forever)
                        .Take(5)
                        .ToList();
                    
                    var recentGames = await GetRecentlyPlayedGames(key, steamId);
                    userStats.RecentlyPlayedGames = recentGames;
                    
                    int recentMinutes = recentGames.Sum(g => g.playtime_2weeks);
                    userStats.Recent2WeeksPlaytimeHours = Math.Round(recentMinutes / 60.0, 1);
                    
                    var playedGames = ownedGames.Where(g => g.playtime_forever > 0).ToList();
                    if (playedGames.Count > 0)
                    {
                        userStats.AveragePlaytimePerGameHours = Math.Round((totalMinutes / (double)playedGames.Count) / 60.0, 1);
                    }
                    
                    userStats.PercentageOfLibraryPlayed = Math.Round((playedGames.Count / (double)ownedGames.Count) * 100, 1);
                }
            }
            catch (Exception)
            {
                // Ignore any errors and return what we have
            }
            
            return userStats;
        }
        
        public async Task<List<SteamUserBadgeInfo>> GetPlayerBadges(string key, string steamId)
        {
            string url = $"https://api.steampowered.com/IPlayerService/GetBadges/v1/?key={key}&steamid={steamId}";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var badgesResponse = JsonSerializer.Deserialize<JsonElement>(json);
                var badgesList = new List<SteamUserBadgeInfo>();
                
                if (badgesResponse.TryGetProperty("response", out var badgesData) && 
                    badgesData.TryGetProperty("badges", out var badges))
                {
                    foreach (var badge in badges.EnumerateArray())
                    {
                        var badgeInfo = new SteamUserBadgeInfo();
                        
                        if (badge.TryGetProperty("badgeid", out var badgeId))
                            badgeInfo.BadgeId = badgeId.GetInt32();
                            
                        if (badge.TryGetProperty("level", out var level))
                            badgeInfo.Level = level.GetInt32();
                            
                        if (badge.TryGetProperty("completion_time", out var completionTime) && completionTime.ValueKind != JsonValueKind.Null)
                            badgeInfo.CompletionTime = DateTimeOffset.FromUnixTimeSeconds(completionTime.GetInt64()).DateTime;
                            
                        if (badge.TryGetProperty("xp", out var xp))
                            badgeInfo.XP = xp.GetInt32();
                            
                        if (badge.TryGetProperty("scarcity", out var scarcity))
                            badgeInfo.Scarcity = scarcity.GetInt32();
                            
                        if (badge.TryGetProperty("appid", out var appId) && appId.ValueKind != JsonValueKind.Null)
                            badgeInfo.AppId = appId.GetInt32();
                            
                        badgesList.Add(badgeInfo);
                    }
                }
                
                return badgesList;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting badges: {e.Message}", e);
            }
        }
    }
}
