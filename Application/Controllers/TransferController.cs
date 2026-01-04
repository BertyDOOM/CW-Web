using CW_Fantasy_App.Data;
using Entities.FootballEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TransferController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransferController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Market()
    {
        List<Player> market = _context.Players
            .Include(p => p.Club).ToList();
        return View(market);
    }
}