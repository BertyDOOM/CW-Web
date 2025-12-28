using CW_Fantasy_App.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.FootballModels
    {
        public class Player : Person
        {
            public string Position { get; set; }
            public int ClubId { get; set; }
            public Club Club { get; set; } = null!;
            public List<PlayerTeam> PlayerTeams { get; set; } = new List<PlayerTeam>();
        }
    }


