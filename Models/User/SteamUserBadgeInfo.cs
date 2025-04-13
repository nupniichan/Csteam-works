using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csteamworks.Models.User
{
    public class SteamUserBadgeInfo
    {
        public int BadgeId { get; set; }
        public int Level { get; set; }
        public DateTime? CompletionTime { get; set; }
        public int XP { get; set; }
        public int Scarcity { get; set; }
        public int? AppId { get; set; }
    }
}
