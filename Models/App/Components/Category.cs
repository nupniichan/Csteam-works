using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csteamworks.Models.App
{
    public class Category
    {
        public int id { get; set; }
        public string description { get; set; }
        
        [JsonIgnore]
        public static Dictionary<int, string> CommonCategories = new Dictionary<int, string>
        {
            { 1, "Multi-player" },
            { 2, "Single-player" },
            { 3, "Co-op" },
            { 4, "Local Co-op" },
            { 5, "Local Multi-player" },
            { 7, "VAC" },
            { 8, "Linux/SteamOS" },
            { 9, "VR Support" },
            { 10, "Workshop" },
            { 13, "Steam Achievements" },
            { 14, "Steam Trading Cards" },
            { 15, "Steam Workshop" },
            { 16, "Steam Leaderboards" },
            { 17, "Remote Play on Tablet" },
            { 18, "Remote Play on TV" },
            { 19, "Remote Play Together" },
            { 20, "Steam Cloud" },
            { 21, "Remote Play on Phone" },
            { 22, "SteamVR Collectibles" },
            { 23, "Game driven Economy" },
            { 24, "Shared/Split Screen" },
            { 25, "VR Only" },
            { 28, "Full controller support" },
            { 29, "Partial Controller Support" },
            { 30, "Steam Trading Cards" },
            { 35, "In-App Purchases" },
            { 36, "Online PvP" },
            { 37, "Online Co-op" },
            { 38, "Shared/Split Screen PvP" },
            { 39, "Shared/Split Screen Co-op" },
            { 40, "Cross-Platform Multiplayer" },
            { 41, "VR Supported" },
            { 42, "PvP" }
        };
        
        [JsonIgnore]
        public bool IsMultiplayer => id == 1 || id == 36 || id == 37 || id == 40;
        
        [JsonIgnore]
        public bool IsSingleplayer => id == 2;
        
        [JsonIgnore]
        public bool IsCoOp => id == 3 || id == 4 || id == 37 || id == 39;
        
        [JsonIgnore]
        public bool HasControllerSupport => id == 28 || id == 29;
        
        [JsonIgnore]
        public bool HasVRSupport => id == 9 || id == 25 || id == 41;
        
        [JsonIgnore]
        public bool HasWorkshop => id == 10 || id == 15;
        
        [JsonIgnore]
        public string CategoryType => GetCategoryType();
        
        private string GetCategoryType()
        {
            if (IsMultiplayer) return "Multiplayer";
            if (IsSingleplayer) return "Singleplayer";
            if (IsCoOp) return "Co-op";
            if (HasControllerSupport) return "Controller";
            if (HasVRSupport) return "VR";
            if (HasWorkshop) return "Workshop";
            
            if (id == 13) return "Features";
            if (id == 14 || id == 30) return "Features";
            if (id == 16) return "Features";
            if (id == 20) return "Features";
            if (id == 35) return "Commerce";
            
            return "Other";
        }
    }
}