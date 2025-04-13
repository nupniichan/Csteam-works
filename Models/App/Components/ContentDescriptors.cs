using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csteamworks.Models.App.Components
{
    public class ContentDescriptors
    {
        [JsonPropertyName("ids")]
        public List<int> Ids { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
}
}
