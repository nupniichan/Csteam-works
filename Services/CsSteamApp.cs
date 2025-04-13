using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using csteamworks.Models.Response;
using csteamworks.Models.App;
using csteamworks.Models.User;

namespace csteamworks.Services
{
    public class CsSteamApp
    {
        public async Task<List<SteamApp>> GetSteamAppIdData(string searchKeyword)
        {
            using HttpClient client = new HttpClient();
            string url = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var appsListResponse = JsonSerializer.Deserialize<AppListResponse>(json);
                var apps = appsListResponse?.applist?.apps ?? new List<SteamApp>();

                return FilterApps(apps, searchKeyword);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting data: {ex.Message}", ex);
            }
        }

        public async Task<AppDetailsResponse> GetSteamAppDetails(int appid, string currency = null)
        {
            string url = string.IsNullOrEmpty(currency)
                ? $"https://store.steampowered.com/api/appdetails?appids={appid}"
                : $"https://store.steampowered.com/api/appdetails?appids={appid}&cc={currency}";

            var result = await GetAppInformation(appid, url);
            
            if (result != null && result.Success && result.Data != null)
            {
                try
                {
                    int playerCount = await GetCurrentPlayerCount(appid);
                    result.Data.CurrentPlayerCount = playerCount;
                }
                catch 
                {
                    // Ignore error for current player count
                }
                
                result.Data.StorePageUrl = $"https://store.steampowered.com/app/{appid}";
                result.Data.CommunityHubUrl = $"https://steamcommunity.com/app/{appid}";
                
                try
                {
                    var storeTags = await GetStorePageTags(appid);
                    if (storeTags != null && storeTags.Count > 0)
                    {
                        var tagsDictionary = new Dictionary<string, string>();
                        foreach (var tag in storeTags)
                        {
                            tagsDictionary.Add(tag, tag);
                        }
                        result.Data.Tags = tagsDictionary;
                    }
                }
                catch
                {
                    // Ignore error for tags
                }
            }
            
            return result;
        }

        private async Task<AppDetailsResponse> GetAppInformation(int appid, string url)
        {
            string currency = null;
            if (url.Contains("cc="))
            {
                var queryParams = url.Split('?')[1].Split('&');
                foreach (var param in queryParams)
                {
                    if (param.StartsWith("cc="))
                    {
                        currency = param.Substring(3);
                        break;
                    }
                }
            }
            
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var appDetailsDictionary = JsonSerializer.Deserialize<Dictionary<string, AppDetailsResponse>>(json);

                if (appDetailsDictionary != null && appDetailsDictionary.TryGetValue(appid.ToString(), out var appResponse) && appResponse.Success)
                {
                    return appResponse;
                }

                // If it can't get those information from main API then we will try to get it from SteamSpy API instead
                var steamSpyInfo = await GetBasicAppInfoFromSteamSpy(appid);
                if (steamSpyInfo != null && steamSpyInfo.Data != null && !string.IsNullOrEmpty(steamSpyInfo.Data.Name))
                {
                    string detailUrl = string.IsNullOrEmpty(currency)
                        ? $"https://store.steampowered.com/api/appdetails?appids={appid}"
                        : $"https://store.steampowered.com/api/appdetails?appids={appid}&cc={currency}";
                    
                    try
                    {
                        response = await client.GetAsync(detailUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            json = await response.Content.ReadAsStringAsync();
                            appDetailsDictionary = JsonSerializer.Deserialize<Dictionary<string, AppDetailsResponse>>(json);
                            
                            if (appDetailsDictionary != null && appDetailsDictionary.TryGetValue(appid.ToString(), out var newAppResponse) && newAppResponse.Success)
                            {
                                return newAppResponse;
                            }
                        }
                    }
                    catch 
                    {
                        // Ignore errors on second try
                    }
                    
                    return steamSpyInfo;
                }
                
                throw new Exception("Can't find with that information.");
            }
            catch (Exception e)
            {
                try 
                {
                    return await GetBasicAppInfoFromSteamSpy(appid);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error while getting data: {e.Message}. Additional error: {ex.Message}", e);
                }
            }
        }
        
        private async Task<AppDetailsResponse> GetBasicAppInfoFromSteamSpy(int appid)
        {
            string steamSpyUrl = $"https://steamspy.com/api.php?request=appdetails&appid={appid}";
            using HttpClient client = new HttpClient();
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(steamSpyUrl);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                
                var steamSpyData = JsonSerializer.Deserialize<JsonElement>(json);
                
                if (steamSpyData.ValueKind != JsonValueKind.Object)
                {
                    throw new Exception("Invalid response from SteamSpy API");
                }
                
                var appResponse = new AppDetailsResponse 
                { 
                    Success = true,
                    Data = new SteamAppDetail()
                };
                
                if (steamSpyData.TryGetProperty("appid", out var appidProp))
                    appResponse.Data.SteamAppId = appidProp.GetInt32();
                
                if (steamSpyData.TryGetProperty("name", out var nameProp))
                    appResponse.Data.Name = nameProp.GetString();
                
                if (steamSpyData.TryGetProperty("developer", out var devProp))
                {
                    appResponse.Data.Developers = new List<string> { devProp.GetString() };
                }
                
                if (steamSpyData.TryGetProperty("publisher", out var pubProp))
                {
                    appResponse.Data.Publishers = new List<string> { pubProp.GetString() };
                }
                
                if (steamSpyData.TryGetProperty("tags", out var tagsProp))
                {
                    var tags = new Dictionary<string, string>();
                    
                    if (tagsProp.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var tag in tagsProp.EnumerateObject())
                        {
                            tags.Add(tag.Name, tag.Name);
                        }
                    }
                    else if (tagsProp.ValueKind == JsonValueKind.Array)
                    {
                        int index = 0;
                        foreach (var tag in tagsProp.EnumerateArray())
                        {
                            if (tag.ValueKind == JsonValueKind.String)
                            {
                                string tagValue = tag.GetString();
                                if (!string.IsNullOrEmpty(tagValue))
                                {
                                    tags.Add(tagValue, tagValue);
                                }
                            }
                            else
                            {
                                tags.Add($"tag_{index}", $"tag_{index}");
                            }
                            index++;
                        }
                    }
                    
                    appResponse.Data.Tags = tags;
                }
                
                appResponse.Data.Genres = new List<Genre>();
                var genreSet = new HashSet<string>();
                
                if (steamSpyData.TryGetProperty("genre", out var genreProp))
                {
                    string genreStr = genreProp.GetString();
                    if (!string.IsNullOrEmpty(genreStr))
                    {
                        string[] genres = genreStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        
                        foreach (string genre in genres)
                        {
                            string trimmedGenre = genre.Trim();
                            if (!string.IsNullOrEmpty(trimmedGenre) && !genreSet.Contains(trimmedGenre))
                            {
                                genreSet.Add(trimmedGenre);
                                appResponse.Data.Genres.Add(new Genre 
                                { 
                                    id = Array.IndexOf(genres, genre).ToString(),
                                    description = trimmedGenre
                                });
                            }
                        }
                    }
                }
                
                if (steamSpyData.TryGetProperty("release_date", out var releaseDateProp))
                {
                    appResponse.Data.ReleaseDate = new ReleaseDate
                    {
                        ComingSoon = false,
                        Date = DateTimeOffset.FromUnixTimeSeconds(releaseDateProp.GetInt64()).ToString("d MMM, yyyy")
                    };
                }
                
                return appResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting data from SteamSpy: {e.Message}", e);
            }
        }
        
        private List<SteamApp> FilterApps(List<SteamApp> apps, string searchKeyword)
        {
            if (string.IsNullOrWhiteSpace(searchKeyword)) return apps;

            return apps
                .Where(app => app.name.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
                .Take(25)
                .ToList();
        }
        
        public async Task<int> GetCurrentPlayerCount(int appId)
        {
            string url = $"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={appId}";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var currentPlayersResponse = JsonSerializer.Deserialize<CurrentPlayersResponse>(json);

                if (currentPlayersResponse != null && currentPlayersResponse.response.result == 1)
                {
                    return currentPlayersResponse.response.player_count;
                }
                else
                {
                    throw new Exception("Can't get current player count of that game");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting data: {e.Message}", e);
            }
        }
        
        public async Task<List<SteamApp>> GetSteamTopGames(int count = 100)
        {
            string url = "https://steamspy.com/api.php?request=top100in2weeks";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var topGamesDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
                var topGames = new List<SteamApp>();

                if (topGamesDict != null)
                {
                    foreach (var game in topGamesDict)
                    {
                        if (topGames.Count >= count) break;
                        
                        var steamApp = new SteamApp();
                        
                        if (int.TryParse(game.Key, out int appId))
                        {
                            steamApp.appid = appId;
                        }
                        
                        if (game.Value.TryGetProperty("name", out var nameElement))
                        {
                            steamApp.name = nameElement.GetString();
                        }
                        
                        if (game.Value.TryGetProperty("developer", out var developerElement))
                        {
                            steamApp.developer = developerElement.GetString();
                        }
                        
                        if (game.Value.TryGetProperty("publisher", out var publisherElement))
                        {
                            steamApp.publisher = publisherElement.GetString();
                        }
                        
                        topGames.Add(steamApp);
                    }
                }

                return topGames;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while getting top games: {e.Message}", e);
            }
        }
        
        private async Task<List<string>> GetStorePageTags(int appId)
        {
            string url = $"https://steamspy.com/api.php?request=appdetails&appid={appId}";
            using HttpClient client = new HttpClient();
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                
                var steamSpyData = JsonSerializer.Deserialize<JsonElement>(json);
                var tags = new List<string>();
                
                if (steamSpyData.TryGetProperty("tags", out var tagsElement))
                {
                    if (tagsElement.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var tagProp in tagsElement.EnumerateObject())
                        {
                            tags.Add(tagProp.Name);
                        }
                    }
                    else if (tagsElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var tag in tagsElement.EnumerateArray())
                        {
                            if (tag.ValueKind == JsonValueKind.String)
                            {
                                string tagValue = tag.GetString();
                                if (!string.IsNullOrEmpty(tagValue))
                                {
                                    tags.Add(tagValue);
                                }
                            }
                        }
                    }
                    
                    return tags;
                }
                
                return await GetTagsFromStorePage(appId);
            }
            catch (Exception)
            {
                return await GetTagsFromStorePage(appId);
            }
        }
        
        private async Task<List<string>> GetTagsFromStorePage(int appId)
        {
            string url = $"https://store.steampowered.com/app/{appId}";
            using HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            client.DefaultRequestHeaders.Add("Cookie", "birthtime=725846401; mature_content=1");
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string html = await response.Content.ReadAsStringAsync();
                
                var tags = new List<string>();
                
                int tagsStartIndex = html.IndexOf("Popular user-defined tags for this product:");
                if (tagsStartIndex > 0)
                {
                    int tagsListStart = html.IndexOf("<a", tagsStartIndex);
                    int tagsListEnd = html.IndexOf("</div>", tagsListStart);
                    
                    if (tagsListStart > 0 && tagsListEnd > tagsListStart)
                    {
                        string tagsSection = html.Substring(tagsListStart, tagsListEnd - tagsListStart);
                        
                        int currentPos = 0;
                        while (true)
                        {
                            int tagStart = tagsSection.IndexOf(">", currentPos);
                            if (tagStart == -1) break;
                            
                            int tagEnd = tagsSection.IndexOf("<", tagStart);
                            if (tagEnd == -1) break;
                            
                            string tag = tagsSection.Substring(tagStart + 1, tagEnd - tagStart - 1).Trim();
                            if (!string.IsNullOrEmpty(tag))
                            {
                                tags.Add(tag);
                            }
                            
                            currentPos = tagEnd;
                            
                            if (tags.Count >= 20) break;
                        }
                    }
                }
                
                if (tags.Count == 0)
                {
                    int appDataStart = html.IndexOf("\"store_tags\":[");
                    if (appDataStart > 0)
                    {
                        int tagsStart = appDataStart + "\"store_tags\":[".Length;
                        int tagsEnd = html.IndexOf("]", tagsStart);
                        
                        if (tagsEnd > tagsStart)
                        {
                            string tagsJson = "[" + html.Substring(tagsStart, tagsEnd - tagsStart) + "]";
                            try
                            {
                                var tagIds = JsonSerializer.Deserialize<List<int>>(tagsJson);
                                
                                foreach (var tagId in tagIds)
                                {

                                    tags.Add($"Tag #{tagId}");
                                }
                            }
                            catch { /* Ignore JSON parsing errors */ }
                        }
                    }
                }
                
                if (tags.Count == 0)
                {
                    try
                    {
                        var gameDetails = await GetSteamAppDetails(appId);
                        if (gameDetails?.Success == true && gameDetails.Data?.Genres != null)
                        {
                            foreach (var genre in gameDetails.Data.Genres)
                            {
                                tags.Add(genre.description);
                            }
                        }
                    }
                    catch { /* Ignore errors */ }
                }
                
                return tags;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
