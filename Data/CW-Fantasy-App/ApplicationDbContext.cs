using CW_Fantasy_App.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CW_Fantasy_App.Entities.BaseEntities;
using Entities.FootballModels;


namespace CW_Fantasy_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<PlayerTeam> PlayerTeams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

//dotnet ef migrations add 
//    --project "C:\Users\Bertan\source\repos\CW-Web\Data\CW Fantasy App\Data.csproj"
//    --startup-project "C:\Users\Bertan\source\repos\CW-Web\Application\Application.csproj"
//    --output-dir "Migrations"

//dotnet ef database update 
//    --project "C:\Users\Bertan\source\repos\CW-Web\Data\CW Fantasy App\Data.csproj"
//    --startup-project "C:\Users\Bertan\source\repos\CW-Web\Application\Application.csproj"

//PS C:\Users\Bertan\source\repos\CW-Web\Application>
//dotnet aspnet-codegenerator identity --project "C:\Users\Bertan\source\repos\CW-Web\Application\Application.csproj" --dbContext "CW_Fantasy_App.Data.ApplicationDbContext" --files "Account.Login;Account.Register;Account.Logout" --force
