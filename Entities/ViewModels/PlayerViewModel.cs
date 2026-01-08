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
        public bool? IsStarter { get; set; }
        public int? SchemePosition { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Age 
        { 
            get
            {
                if (DateOfBirth == null)
                {
                    return 0;
                }
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
