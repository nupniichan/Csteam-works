using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steamapi.Models.App
{
    public class OwnedGames
    {
        public int game_count { get; set; }
        public List<OwnedGame> games { get; set; }
    }
}
