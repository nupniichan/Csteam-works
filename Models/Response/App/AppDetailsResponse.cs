using csteamworks.Models.User;
using System.Text.Json.Serialization;

namespace csteamworks.Models.Response
{
    public class AppDetailsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public SteamAppDetail Data { get; set; }
    }
}