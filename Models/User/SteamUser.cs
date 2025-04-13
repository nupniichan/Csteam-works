using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace csteamworks.Models.User
{
    public class SteamUser
    {
        public string steamid { get; set; }
        public int communityvisibilitystate { get; set; }
        public int profilestate { get; set; }
        public string personaname { get; set; }
        public string profileurl { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public string avatarhash { get; set; }
        public long lastlogoff { get; set; }
        public int personastate { get; set; }
        public string realname { get; set; }
        public string primaryclanid { get; set; }
        public long timecreated { get; set; }
        public int personastateflags { get; set; }
        public string loccountrycode { get; set; }
        public string locstatecode { get; set; }
        public string loccityid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gameextrainfo { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gameid { get; set; }
        
        [JsonIgnore]
        public DateTime LastLogoffDate => DateTimeOffset.FromUnixTimeSeconds(lastlogoff).DateTime;
        
        [JsonIgnore]
        public DateTime AccountCreatedDate => DateTimeOffset.FromUnixTimeSeconds(timecreated).DateTime;
        
        [JsonIgnore]
        public string OnlineStatus => GetOnlineStatus();
        
        [JsonIgnore]
        public string VisibilityState => GetVisibilityState();
        
        private string GetOnlineStatus()
        {
            return personastate switch
            {
                0 => "Offline",
                1 => "Online",
                2 => "Busy",
                3 => "Away",
                4 => "Snooze",
                5 => "Looking to Trade",
                6 => "Looking to Play",
                _ => "Unknown"
            };
        }
        
        private string GetVisibilityState()
        {
            return communityvisibilitystate switch
            {
                1 => "Private",
                2 => "Friends Only",
                3 => "Public",
                _ => "Unknown"
            };
        }
    }
}
