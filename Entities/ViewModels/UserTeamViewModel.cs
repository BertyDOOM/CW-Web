using Entities.FootballEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class UserTeamViewModel
    {   
        public string UserName { get; set; } //без @.bg...
        public int UserPoints { get; set; }
        public int UserCoins { get; set; }
        public string FavoriteClubName { get; set; }
        public string FavoriteClubCrestUrl { get; set; }
        public string TeamName { get; set; }
        public CoachViewModel Coach { get; set; }
        public List<PlayerViewModel> StarterPlayers { get; set; }
        public List<PlayerViewModel> BenchPlayers { get; set; }
    }
}
