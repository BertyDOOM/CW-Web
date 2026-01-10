using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class MarketUserTeamViewModel
    {
        public int TeamId { get; set; }
        public int UserCoins { get; set; }
        public int StarterCount { get; set; }
        public int BenchCount { get; set; }
        public int? OwnedCoachId { get; set; }
        public List<int> OwnedPlayerIds { get; set; }
    }
}
