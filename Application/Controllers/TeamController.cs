using CW_Fantasy_App.Data;
using Entities.FootballEntities;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
     
        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet] //default / и при зареждане на страницата
        public IActionResult Create()
        {
            var model = new CreateTeamViewModel
            {
                Clubs = _context.Clubs
            .Where(c => !c.IsDeleted)
            .ToList()
            };
            return View(model);
        }

        [HttpPost] //след submit
        public async Task<IActionResult> Create(CreateTeamViewModel model)
        {
            var team = new Team
            {
                Name = model.Name,
                UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                FavoriteClubId = model.FavoriteClubId,
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
