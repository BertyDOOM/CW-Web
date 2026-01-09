using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class PlayerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string KitUrl { get; set; }
        public string ClubUrl { get; set; }
        public bool? IsStarter { get; set; }
        public int? SchemePosition { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public int? Age { get; set; }

        public static int? SetAge(DateTime? dateOfBirth)
        {
            if (dateOfBirth == null)
            {
                return null;
            }
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Value.Year;
            if (dateOfBirth.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}

