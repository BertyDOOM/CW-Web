using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Options
{
    public class FootballDataOptions
    {
        public string ApiKey { get; set; } = null!;
        public string CustomKeyApiName { get; set; } = null!;
        public string BaseURL { get; set; } = null!;
        public FootballEndpoints Endpoints { get; set; } = new();
    }

    public class FootballEndpoints
    {
        public string TeamDetails { get; set; } = null!;
    }
}
