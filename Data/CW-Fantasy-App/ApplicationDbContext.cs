using CW_Fantasy_App.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CW_Fantasy_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            // CreatedAt/UpdatedAt
            return base.SaveChanges();
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
