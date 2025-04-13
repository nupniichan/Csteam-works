using csteamworks.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csteamworks.Models.User
{
    public class SteamUserStats
    {
        public string SteamId { get; set; }
        public string PlayerName { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime AccountCreated { get; set; }
        public string OnlineStatus { get; set; }
        public int TotalGamesOwned { get; set; }
        public double TotalPlaytimeHours { get; set; }
        public double Recent2WeeksPlaytimeHours { get; set; }
        public double AveragePlaytimePerGameHours { get; set; }
        public double PercentageOfLibraryPlayed { get; set; }
        public List<OwnedGame> MostPlayedGames { get; set; } = new List<OwnedGame>();
        public List<Game> RecentlyPlayedGames { get; set; } = new List<Game>();
    }
}
