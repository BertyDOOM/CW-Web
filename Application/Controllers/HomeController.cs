using Application.Models;
using CW_Fantasy_App.Data;
using CW_Fantasy_App.Entities.Entities;
using Entities.FootballEntities;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        var model = new UserTeamViewModel
        {
            UserName = user.UserName.Split('@')[0],
            UserCoins = user.Coins,
            UserPoints = user.Points,
            FavoriteClubCrestUrl = team.FavoriteClub?.CrestUrl,
            FavoriteClubName = team.FavoriteClub?.Name,
            TeamName = team.Name,
            Coach = team.Coach,
            Players = team.PlayerTeams.Select(pt => new PlayerViewModel
            {
                Name = pt.Player.Name,
                Position = pt.Player.Position,
                KitUrl = pt.Player.Club?.KitUrl,
                IsStarter = pt.IsStarter,
                DateOfBirth =  pt.Player.DateOfBirth,

            }).OrderBy(pt => pt.IsStarter == true) //титул€рите?
            .ToList()
        };

        return View(model);
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
