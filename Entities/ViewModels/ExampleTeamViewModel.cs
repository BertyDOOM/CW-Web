using Entities.FootballEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class TeamViewModel
    {   
        public Coach Coach { get; set; }
        public List<PlayerViewModel> Players { get; set; } // 11 стартови играчи

    }
}
