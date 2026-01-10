using Application.Models;
using CW_Fantasy_App.Data;
using CW_Fantasy_App.Entities.Entities;
using Entities.FootballEntities;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Application.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            ViewBag.ShowLogin = true;
            ViewBag.ShowCreateTeam = false;
            return View("Welcome");
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return RedirectToAction("Welcome");
        }
        var team = _context.Teams
                .Include(t => t.FavoriteClub)
                .Include(t => t.Coach)
                .Include(t => t.PlayerTeams)
                    .ThenInclude(pt => pt.Player)
                        .ThenInclude(p => p.Club)
                .FirstOrDefault(t => t.UserId == userId && !t.IsDeleted);


        if (team == null)
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowCreateTeam = true;
            return View("Welcome");
        }
        var starterPlayers = team.PlayerTeams
    .Where(pt => pt.IsStarter == true)
    .OrderBy(pt => pt.SchemePosition)
    .Select(pt => new PlayerViewModel
    {
        Id = pt.Player.Id,
        Name = pt.Player.Name,
        Position = pt.Player.Position,
        KitUrl = pt.Player.Club?.KitUrl,
        ClubUrl = pt.Player.Club?.CrestUrl,
        SchemePosition = pt.SchemePosition,
        DateOfBirth = pt.Player.DateOfBirth,
        Nationality = pt.Player.Nationality,
        Age = PlayerViewModel.SetAge(pt.Player.DateOfBirth),
        IsStarter = pt.IsStarter
    })
    .Take(11)
    .ToList();


        var benchPlayers = team.PlayerTeams
                .Where(pt => pt.IsStarter != true)
                .OrderBy(pt => pt.Player.Id)
                .Select(pt => new PlayerViewModel
                {
                    Id = pt.Player.Id,
                    Name = pt.Player.Name,
                    Position = pt.Player.Position,
                    KitUrl = pt.Player.Club?.KitUrl,
                    ClubUrl = pt.Player.Club?.CrestUrl,
                    SchemePosition = pt.SchemePosition,
                    DateOfBirth = pt.Player.DateOfBirth,
                    Nationality = pt.Player.Nationality,
                    Age = PlayerViewModel.SetAge(pt.Player.DateOfBirth),
                    IsStarter = pt.IsStarter
                })
                .ToList();

        CoachViewModel? coachViewModel = null;

        if (team.Coach != null)
        {
            coachViewModel = new CoachViewModel
            {
                Id = team.Coach.Id,
                Name = team.Coach.Name,
                ClubUrl = team.Coach.Club?.CrestUrl, 
                DateOfBirth = team.Coach.DateOfBirth,
                Nationality = team.Coach.Nationality,
                Age = CoachViewModel.SetAge(team.Coach.DateOfBirth)
            };
        }

        var model = new UserTeamViewModel
        {
            UserName = user.UserName.Split('@')[0],
            UserCoins = user.Coins,
            UserPoints = user.Points,
            FavoriteClubCrestUrl = team.FavoriteClub?.CrestUrl,
            FavoriteClubName = team.FavoriteClub?.Name,
            TeamName = team.Name,
            Coach = coachViewModel,
            StarterPlayers = starterPlayers,
            BenchPlayers = benchPlayers
            
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> SchemePositionChanger([FromBody] SwapPositionsDto dto)
    {
        var player1 = _context.PlayerTeams.FirstOrDefault(pt => pt.PlayerId == dto.Pos1 && pt.IsStarter == true);
        var player2 = _context.PlayerTeams.FirstOrDefault(pt => pt.PlayerId == dto.Pos2 && pt.IsStarter == true);

        if (player1 == null || player2 == null)
            return BadRequest("Invalid player positions");

        int? temp = player1.SchemePosition;
        player1.SchemePosition = player2.SchemePosition;
        player2.SchemePosition = temp;

        await _context.SaveChangesAsync();

        return Ok(new[]
        {
        new { PlayerId = player1.PlayerId, SchemePosition = player1.SchemePosition },
        new { PlayerId = player2.PlayerId, SchemePosition = player2.SchemePosition }
    });
    }

    [HttpPost]
    public async Task<IActionResult> MakeStarter([FromBody] MakeStarterDto dto) 
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var team = await _context.Teams
        .FirstOrDefaultAsync(t => t.UserId == userId);

        var newStarter = await _context.PlayerTeams
        .FirstOrDefaultAsync(pt =>
            pt.TeamId == team.Id &&
            pt.PlayerId == dto.PlayerId);

        var oldStarter = await _context.PlayerTeams
                .FirstOrDefaultAsync(pt =>
            pt.SchemePosition == dto.SchemePosition &&
            pt.Team.UserId == userId &&
            pt.IsStarter == true);

        if (oldStarter == null)
        {
            newStarter.IsStarter = true;
            newStarter.SchemePosition = dto.SchemePosition;
            await _context.SaveChangesAsync();
            return Ok();
        }

        oldStarter.IsStarter = false;
        oldStarter.SchemePosition = null;

        newStarter.IsStarter = true;
        newStarter.SchemePosition = dto.SchemePosition;

        await _context.SaveChangesAsync();
        return Ok();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
