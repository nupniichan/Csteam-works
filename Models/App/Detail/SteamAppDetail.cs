using csteamworks.Models.App;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using csteamworks.Models.App.Components;

namespace csteamworks.Models.App.Detail
{
    public class SteamAppDetail
    {
        [JsonPropertyName("steam_appid")]
        public int SteamAppId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("required_age")]
        public object RequiredAge { get; set; }

        [JsonPropertyName("is_free")]
        public bool IsFree { get; set; }

        [JsonPropertyName("controller_support")]
        public string ControllerSupport { get; set; }

        [JsonPropertyName("dlc")]
        public List<int> Dlc { get; set; }

        [JsonPropertyName("detailed_description")]
        public string DetailedDescription { get; set; }

        [JsonPropertyName("about_the_game")]
        public string AboutTheGame { get; set; }

        [JsonPropertyName("short_description")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("supported_languages")]
        public string SupportedLanguages { get; set; }

        [JsonPropertyName("reviews")]
        public string Reviews { get; set; }

        [JsonPropertyName("header_image")]
        public string HeaderImage { get; set; }

        [JsonPropertyName("price_overview")]
        public PriceOverview PriceOverview { get; set; }

        [JsonPropertyName("pc_requirements")]
        public PCRequirements PcRequirements { get; set; }

        [JsonPropertyName("mac_requirements")]
        public PCRequirements MacRequirements { get; set; }
        
        [JsonPropertyName("linux_requirements")]
        public PCRequirements LinuxRequirements { get; set; }

        [JsonPropertyName("platforms")]
        public Platforms Platforms { get; set; }

        [JsonPropertyName("metacritic")]
        public Metacritic Metacritic { get; set; }

        [JsonPropertyName("categories")]
        public List<Category> Categories { get; set; }

        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonPropertyName("screenshots")]
        public List<Screenshot> Screenshots { get; set; }

        [JsonPropertyName("movies")]
        public List<Movie> Movies { get; set; }
        
        [JsonPropertyName("release_date")]
        public ReleaseDate ReleaseDate { get; set; }
        
        [JsonPropertyName("developers")]
        public List<string> Developers { get; set; }
        
        [JsonPropertyName("publishers")]
        public List<string> Publishers { get; set; }
        
        [JsonPropertyName("background")]
        public string Background { get; set; }
        
        [JsonPropertyName("background_raw")]
        public string BackgroundRaw { get; set; }
        
        [JsonPropertyName("content_descriptors")]
        public ContentDescriptors ContentDescriptors { get; set; }
        
        [JsonPropertyName("recommendations")]
        public Recommendations Recommendations { get; set; }
        
        [JsonPropertyName("tags")]
        public Dictionary<string, string> Tags { get; set; }
        
        [JsonIgnore]
        public int CurrentPlayerCount { get; set; }
        
        [JsonIgnore]
        public string StorePageUrl { get; set; }
        
        [JsonIgnore]
        public string CommunityHubUrl { get; set; }
        
        [JsonIgnore]
        public bool HasMultiplayerSupport => Categories?.Any(c => c.IsMultiplayer || c.IsCoOp) ?? false;
        
        [JsonIgnore]
        public bool HasSingleplayerSupport => Categories?.Any(c => c.IsSingleplayer) ?? false;
        
        [JsonIgnore]
        public bool HasCoOpSupport => Categories?.Any(c => c.IsCoOp) ?? false;
        
        [JsonIgnore]
        public bool HasVRSupport => Categories?.Any(c => c.HasVRSupport) ?? false;
        
        [JsonIgnore]
        public bool HasControllerSupport => !string.IsNullOrEmpty(ControllerSupport) || 
                                          (Categories?.Any(c => c.HasControllerSupport) ?? false);
        
        [JsonIgnore]
        public bool HasWorkshopSupport => Categories?.Any(c => c.HasWorkshop) ?? false;
        
        [JsonIgnore]
        public string PrimaryGenre => GetPrimaryGenre();
        
        [JsonIgnore]
        public List<string> MainGenres => GetMainGenres();
        
        [JsonIgnore]
        public int? MetacriticScore => Metacritic?.score;
        
        [JsonIgnore]
        public int RecommendationCount => Recommendations?.total ?? 0;
        
        [JsonIgnore]
        public string FormattedReleaseDate => ReleaseDate?.Date;
        
        [JsonIgnore]
        public bool IsUpcoming => ReleaseDate?.ComingSoon ?? false;
        
        [JsonIgnore]
        public string PrimaryDeveloper => Developers?.FirstOrDefault();
        
        [JsonIgnore]
        public string PrimaryPublisher => Publishers?.FirstOrDefault();
        
        [JsonIgnore]
        public List<string> TagList => Tags?.Keys.ToList() ?? new List<string>();
        
        [JsonIgnore]
        public int ScreenshotCount => Screenshots?.Count ?? 0;
        
        [JsonIgnore]
        public int MovieCount => Movies?.Count ?? 0;
        
        [JsonIgnore]
        public int DLCCount => Dlc?.Count ?? 0;
        
        [JsonIgnore]
        public SystemRequirementsDetail MinimumSystemRequirements => PcRequirements?.MinimumRequirements;
        
        [JsonIgnore]
        public SystemRequirementsDetail RecommendedSystemRequirements => PcRequirements?.RecommendedRequirements;
        
        private string GetPrimaryGenre()
        {
            if (Genres == null || Genres.Count == 0)
                return null;
                
            var mainGenre = Genres.FirstOrDefault(g => g.IsMainGenre);
            if (mainGenre != null)
                return mainGenre.description;
                
            return Genres[0].description;
        }
        
        private List<string> GetMainGenres()
        {
            if (Genres == null || Genres.Count == 0)
                return new List<string>();
                
            return Genres
                .Select(g => g.description)
                .ToList();
        }
    }
}
