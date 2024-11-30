using steamapi.Models.App;

namespace steamapi.Models.Player
{
    public class RecentlyPlayedGames
    {
        public int total_count { get; set; }
        public List<Game> games { get; set; }
    }
}