using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class BuyPlayerDto
    {
        public int PlayerId { get; set; }
        public int? SchemePosition { get; set; }
        public int Price { get; set; }
    }
}
