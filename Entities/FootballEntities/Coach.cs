using CW_Fantasy_App.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.FootballEntities
{
    public class Coach : Person
    {
        public int? ClubId { get; set; }
        public Club? Club { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
