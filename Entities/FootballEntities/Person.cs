using CW_Fantasy_App.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.FootballEntities
{
    public class Person : BaseEntity
    {
        public int ApiId { get; set; }
        public string Name { get; set; } = null!;
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
