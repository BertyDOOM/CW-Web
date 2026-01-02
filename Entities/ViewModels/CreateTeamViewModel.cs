using Entities.FootballEntities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class CreateTeamViewModel
    {
        public string Name { get; set; }
        public int FavoriteClubId { get; set; }
        public IEnumerable<Club> Clubs { get; set; }
    }
}
