using Services.DTOs;

namespace CW_Fantasy_App.Services
{
    public class TeamApiResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Crest { get; set; } = null!;
        public CoachDto Coach { get; set; } = null!;
        public List<PlayerDto> Squad { get; set; } = new();
    }
}
