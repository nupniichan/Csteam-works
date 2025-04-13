using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csteamworks.Models.App
{
    public class Genre
    {
        public string id { get; set; }
        public string description { get; set; }
        
        [JsonIgnore]
        public static Dictionary<string, string> CommonGenres = new Dictionary<string, string>
        {
            { "1", "Action" },
            { "2", "Strategy" },
            { "3", "RPG" },
            { "4", "Casual" },
            { "5", "Racing" },
            { "6", "Sports" },
            { "7", "Simulation" },
            { "8", "Adventure" },
            { "9", "Indie" },
            { "10", "Massively Multiplayer" },
            { "23", "Arcade" },
            { "25", "Education" },
            { "28", "Puzzle" },
            { "37", "Visual Novel" },
            { "53", "Free to Play" },
            { "57", "Early Access" },
            { "70", "Animation & Modeling" }
        };
        
        [JsonIgnore]
        public bool IsMainGenre => IsCommonMainGenre(id);
        
        private bool IsCommonMainGenre(string genreId)
        {
            return genreId == "1" || genreId == "2" || genreId == "3" || 
                   genreId == "5" || genreId == "7" || genreId == "8" || 
                   genreId == "23" || genreId == "28";
        }
    }
}