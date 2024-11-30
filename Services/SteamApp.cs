using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.Caching;
using csteamworks.Models.Response;
using csteamworks.Models.App;

namespace csteamworks.Services
{
    public class SteamApp
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private const string SteamAppsCacheKey = "SteamAppsCache";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(6);

        public async Task<List<Models.App.SteamApp>> GetSteamAppIdData(string searchKeyword)
        {
            if (_cache.Contains(SteamAppsCacheKey))
            {
                var cachedApps = (List<Models.App.SteamApp>)_cache.Get(SteamAppsCacheKey);
                return FilterApps(cachedApps, searchKeyword);
            }

            using HttpClient client = new HttpClient();
            string url = "https://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var appsListResponse = JsonSerializer.Deserialize<AppListResponse>(json);
                var apps = appsListResponse?.applist?.apps ?? new List<Models.App.SteamApp>();

                _cache.Set(SteamAppsCacheKey, apps, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(CacheDuration) });

                return FilterApps(apps, searchKeyword);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu: {ex.Message}", ex);
            }
        }

        public async Task<AppDetailsResponse> GetSteamAppDetails(int appid, string currency = "vn")
        {
            string url = currency == "vn"
                ? $"https://store.steampowered.com/api/appdetails?appids={appid}"
                : $"https://store.steampowered.com/api/appdetails?appids={appid}&cc={currency}";

            return await GetAppInformation(appid, url);
        }

        private async Task<AppDetailsResponse> GetAppInformation(int appid, string url)
        {
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var appDetailsDictionary = JsonSerializer.Deserialize<Dictionary<string, AppDetailsResponse>>(json);

                if (appDetailsDictionary != null && appDetailsDictionary.TryGetValue(appid.ToString(), out var appResponse))
                {
                    return appResponse;
                }

                throw new Exception("Không tìm thấy ứng dụng với ID này.");
            }
            catch (Exception e)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu: {e.Message}", e);
            }
        }
        private List<Models.App.SteamApp> FilterApps(List<Models.App.SteamApp> apps, string searchKeyword)
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
                    throw new Exception("Không thể lấy số lượng người chơi.");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Lỗi khi gọi API: {e.Message}", e);
            }
        }
    }
}
