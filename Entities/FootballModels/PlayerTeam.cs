using CW_Fantasy_App.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.FootballModels
{
        public class PlayerTeam : BaseEntity
        {
            public int PlayerId { get; set; }
            public Player Player { get; set; } = null!;
            public int TeamId { get; set; }
            public Team Team { get; set; } = null!;
            public bool? IsStarter { get; set; }
            public int? SchemePosition { get; set; }
        }
}

