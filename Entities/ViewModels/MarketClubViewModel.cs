using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class MarketClubViewModel
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }

        public CoachViewModel Coach { get; set; }
        public List<MarketPlayerViewModel> Players { get; set; }
    }
}
