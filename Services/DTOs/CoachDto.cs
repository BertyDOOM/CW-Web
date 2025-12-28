using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CoachDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
