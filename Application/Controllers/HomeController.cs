using Application.Models;
using CW_Fantasy_App.Data;
using CW_Fantasy_App.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Claims;

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

        var team = _context.Teams
                .Include(t => t.PlayerTeams)
                    .ThenInclude(pt => pt.Player)
                .Include(t => t.Coach)
                .Include(t => t.FavoriteClub)
                .FirstOrDefault(t => t.UserId == userId && !t.IsDeleted);

        if (team == null) 
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowCreateTeam = true;
            return View("Welcome");
        }
            return View(); //отбора
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
