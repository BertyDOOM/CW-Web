using CW_Fantasy_App.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.FootballEntities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int UserId { get; set; }
        public int? FavoriteClubId { get; set; }
        public Club? FavoriteClub { get; set; }
        public int? CoachId { get; set; }
        public Coach? Coach { get; set; }
        public List<PlayerTeam> PlayerTeams { get; set; } = new List<PlayerTeam>();
    }
}
