﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace csteamworks.Models.App
{
    public class OwnedGame
    {
        public int appid { get; set; }
        public int playtime_forever { get; set; }
        public int playtime_windows_forever { get; set; }
        public int playtime_mac_forever { get; set; }
        public int playtime_linux_forever { get; set; }
        public int playtime_deck_forever { get; set; }
        public long rtime_last_played { get; set; }
        public int playtime_disconnected { get; set; }
        public string name { get; set; }
        [JsonPropertyName("img_icon_url")]
        public string img_icon_url { get; set; }
    }
}
