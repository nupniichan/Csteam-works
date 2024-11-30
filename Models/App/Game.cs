﻿namespace steamapi.Models.App
{
    public class Game
    {
        public int appid { get; set; }
        public string name { get; set; }
        public int playtime_2weeks { get; set; }
        public int playtime_forever { get; set; }
        public string img_icon_url { get; set; }
        public int playtime_windows_forever { get; set; }
        public int playtime_mac_forever { get; set; }
        public int playtime_linux_forever { get; set; }
        public int playtime_deck_forever { get; set; }
    }
}