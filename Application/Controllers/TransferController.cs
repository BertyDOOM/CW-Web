using CW_Fantasy_App.Data;
using Entities.FootballEntities;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using System.Security.Claims;

public class TransferController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransferController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Market()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Unauthorized();

        // 2️⃣ Взимаме отбора на user-а
        var team = await _context.Teams
            .Include(t => t.PlayerTeams)
                .ThenInclude(pt => pt.Player)
            .Include(t => t.Coach)
            .FirstOrDefaultAsync(t => t.UserId == userId);

        var ownedPlayerIds = team?.PlayerTeams.Where(pt => !pt.IsDeleted).Select(pt => pt.PlayerId).ToList() ?? new List<int>();

        var marketUserTeamVm = new MarketUserTeamViewModel
        {
            TeamId = team?.Id ?? 0,
            UserCoins = user.Coins,
            StarterCount = team?.PlayerTeams.Count(pt => pt.IsStarter == true && pt.IsDeleted != true) ?? 0,
            BenchCount = team?.PlayerTeams.Count(pt => pt.IsStarter == true && !pt.IsDeleted) ?? 0,
            OwnedPlayerIds = ownedPlayerIds,
            OwnedCoachId = team.CoachId
        };

        // 3️⃣ Взимаме всички клубове + играчи + треньори
        var clubs = await _context.Clubs
            .Include(c => c.Coach)
            .Include(c => c.Players)
            .ToListAsync();

        var clubVms = clubs.Select(c => new MarketClubViewModel
        {
            ClubId = c.Id,
            ClubName = c.Name,
            Coach = c.Coach != null ? new CoachViewModel
            {
                Id = c.Coach.Id,
                Name = c.Coach.Name,
                DateOfBirth = c.Coach.DateOfBirth,
                ClubUrl = c.CrestUrl,
                Nationality = c.Coach.Nationality,
                Age = CoachViewModel.SetAge(c.Coach.DateOfBirth)
            } : null,
            Players = c.Players
                .Where(p => !ownedPlayerIds.Contains(p.Id) && !p.IsDeleted) // <- игнорирани са soft-deleted
                .Select(p => new MarketPlayerViewModel
                {
                    Price = 1000,
                    IsOwned = false,
                    Info = new PlayerViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Position = p.Position,
                        KitUrl = p.Club.KitUrl,
                        ClubUrl = c.CrestUrl,
                        DateOfBirth = p.DateOfBirth,
                        Nationality = p.Nationality,
                        Age = PlayerViewModel.SetAge(p.DateOfBirth)
                    }
                }).ToList()

        }).ToList();

        var pageVm = new MarketPageViewModel
        {
            UserTeam = marketUserTeamVm,
            Clubs = clubVms
        };

        return View(pageVm);
    }

    [HttpPost]
    public async Task<IActionResult> BuyPlayer([FromBody] BuyPlayerDto dto)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Unauthorized();

        var player = await _context.Players.FindAsync(dto.PlayerId);
        if (player == null) return BadRequest("Player not found");

        if (user.Coins < dto.Price) return BadRequest("Not enough coins");

        var team = await _context.Teams
            .Include(t => t.PlayerTeams)
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (team == null) return BadRequest("Team not found");

        
        var positionTaken = team.PlayerTeams.Any(pt => pt.SchemePosition == dto.SchemePosition && pt.IsStarter ==true);

        team.PlayerTeams.Add(new PlayerTeam
        {
            PlayerId = player.Id,
            IsStarter = !positionTaken,          
            SchemePosition = positionTaken ? null : dto.SchemePosition
        });

        user.Coins -= dto.Price;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Player bought",
            playerId = player.Id,
            newCoins = user.Coins,
            isStarter = !positionTaken
        });
    }

    [HttpPost]
    public async Task<IActionResult> RecruitCoach([FromBody] RecruitCoachDto dto)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Взимаме отбора на user-а
        var team = await _context.Teams
            .Include(t => t.Coach)
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (team == null) return BadRequest("Team not found");

        if (team.Coach != null)
            return BadRequest("Your team already has a coach.");

        var coach = await _context.Coaches.FindAsync(dto.CoachId);
        if (coach == null) return NotFound("Coach not found.");

        // Слагаме треньора на отбора
        team.CoachId = coach.Id;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Coach recruited successfully!" });
    }

}