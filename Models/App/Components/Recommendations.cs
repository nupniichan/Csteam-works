using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csteamworks.Models.App.Components
{
    public class Recommendations
    {
        [JsonPropertyName("total")]
        public int total { get; set; }
    }
}
