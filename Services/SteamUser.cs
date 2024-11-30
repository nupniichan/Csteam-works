using csteamworks.Models.App;
using csteamworks.Models.Player;
using csteamworks.Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace csteamworks.Services
{
    public class SteamUser
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
                throw new Exception("Không tìm thấy user với username này.");
            }
            catch (Exception e)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu: {e.Message}", e);
            }
        }

        public async Task<SteamPlayer> GetPlayerSummaries(string key, string steamId)
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
                throw new Exception("Không tìm thấy user với steamid này.");
            }
            catch (Exception e)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu: {e.Message}", e);
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
                throw new Exception($"Lỗi khi lấy dữ liệu: {e.Message}", e);
            }
        }

        public async Task<List<OwnedGame>> GetOwnedGames(string key, string steamId)
        {
            string url = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={key}&steamid={steamId}&format=json";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var ownedGamesResponse = JsonSerializer.Deserialize<OwnedGamesResponse>(json);

                return ownedGamesResponse?.response?.games ?? new List<OwnedGame>();
            }
            catch (Exception e)
            {
                throw new Exception($"Lỗi khi lấy dữ liệu: {e.Message}", e);
            }
        }
    }
}
