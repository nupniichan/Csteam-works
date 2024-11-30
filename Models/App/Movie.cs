namespace csteamworks.Models.App
{
    public class Movie
    {
        public int id { get; set; }
        public string name { get; set; }
        public string thumbnail { get; set; }
        public MovieFormats webm { get; set; }
        public MovieFormats mp4 { get; set; }
        public bool highlight { get; set; }
    }
}