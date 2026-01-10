using CW_Fantasy_App.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.FootballEntities
{
    public class Club : BaseEntity
    {
        public int ApiId { get; set; }
        public string Name { get; set; } = null!;
        public string? CrestUrl { get; set; }
        public string? KitUrl { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public Coach Coach { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
