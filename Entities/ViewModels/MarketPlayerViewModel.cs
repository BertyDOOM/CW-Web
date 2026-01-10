using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class MarketPlayerViewModel
    {
        public int Price { get; set; } = 1000;
        public bool IsOwned { get; set; }
        public PlayerViewModel Info { get; set; }
    }
}

