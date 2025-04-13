using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csteamworks.Models.App
{
    public class Tag
    {
        public string name { get; set; }
        public int count { get; set; }
        public bool browseable { get; set; }
        
        [JsonIgnore]
        public static List<string> PopularTags = new List<string>
        {
            "Action", "Adventure", "Casual", "Indie", "Massively Multiplayer", "Racing", "RPG", 
            "Simulation", "Sports", "Strategy", "Singleplayer", "Multiplayer", "Co-op", 
            "Open World", "First-Person", "Third Person", "Free to Play", "Early Access",
            "2D", "3D", "Horror", "Survival", "Shooter", "Platformer", "Puzzle", "Story Rich",
            "Atmospheric", "Fantasy", "Sci-fi", "Anime", "Cartoon", "Difficult", "Relaxing", 
            "Tactical", "Building", "Base Building", "Management", "Sandbox", "Family Friendly",
            "Comedy", "FPS", "Space", "Turn-Based", "Physics", "VR", "Action RPG", "JRPG", 
            "Hack and Slash", "RTS", "Roguelike", "Crafting", "Online Co-Op", "Local Co-Op",
            "PvP", "PvE", "Card Game", "Retro", "Classic", "Pixel Graphics", "Arcade",
            "Realistic", "Combat", "Mods", "MOBA", "Tower Defense", "Mystery", "Dark",
            "Stealth", "War", "Historical", "Robots", "Zombies", "Superhero", "Dragons",
            "Pirates", "Ninja", "Aliens", "Time Travel", "Magic", "Music", "Rhythm",
            "Fishing", "Hunting", "Farming", "Cooking", "Racing", "Flight", "Naval",
            "Tanks", "Space Sim", "Submarine", "Trains", "Transport", "Economy",
            "Trading", "Education", "Programming", "Visual Novel", "Dating Sim"
        };
        
        [JsonIgnore]
        public static Dictionary<string, string> TagCategories = new Dictionary<string, string>
        {
            { "Action", "Genre" },
            { "Adventure", "Genre" },
            { "Casual", "Genre" },
            { "Indie", "Genre" },
            { "Massively Multiplayer", "Genre" },
            { "Racing", "Genre" },
            { "RPG", "Genre" },
            { "Simulation", "Genre" },
            { "Sports", "Genre" },
            { "Strategy", "Genre" },
            
            { "Singleplayer", "Player Mode" },
            { "Multiplayer", "Player Mode" },
            { "Co-op", "Player Mode" },
            { "Online Co-Op", "Player Mode" },
            { "Local Co-Op", "Player Mode" },
            { "PvP", "Player Mode" },
            { "PvE", "Player Mode" },
            
            { "Open World", "Setting" },
            { "Fantasy", "Setting" },
            { "Sci-fi", "Setting" },
            { "Space", "Setting" },
            { "Cyberpunk", "Setting" },
            { "Post-apocalyptic", "Setting" },
            { "Historical", "Setting" },
            { "Medieval", "Setting" },
            { "Modern", "Setting" },
            { "Western", "Setting" },
            
            { "First-Person", "Perspective" },
            { "Third Person", "Perspective" },
            { "Top-Down", "Perspective" },
            { "Side View", "Perspective" },
            { "Isometric", "Perspective" },
            
            { "2D", "Visual" },
            { "3D", "Visual" },
            { "Pixel Graphics", "Visual" },
            { "Anime", "Visual" },
            { "Cartoon", "Visual" },
            { "Realistic", "Visual" },
            { "Retro", "Visual" },
            
            { "Horror", "Theme" },
            { "Survival", "Theme" },
            { "Mystery", "Theme" },
            { "Comedy", "Theme" },
            { "Dark", "Theme" },
            { "War", "Theme" },
            
            { "VR", "Technology" },
            { "Early Access", "Development" },
            { "Free to Play", "Price" }
        };
        
        [JsonIgnore]
        public string Category => GetTagCategory();
        
        private string GetTagCategory()
        {
            if (TagCategories.TryGetValue(name, out string category))
                return category;
                
            return "Other";
        }
    }
} 