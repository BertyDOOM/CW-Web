using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class MarketPageViewModel
    {
        public MarketUserTeamViewModel UserTeam { get; set; }
        public List<MarketClubViewModel> Clubs { get; set; }
    }
}
