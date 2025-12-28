using CW_Fantasy_App.Data;
using CW_Fantasy_App.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// Cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// HttpClient + FootballDataService
builder.Services.AddHttpClient<FootballDataService>();
builder.Services.AddScoped<FootballDataService>();

// FootballDataOptions
builder.Services.Configure<FootballDataOptions>(
    builder.Configuration.GetSection("FootballDataOptions"));

var app = builder.Build();

// Извикване на FootballDataService
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var footballService = services.GetRequiredService<FootballDataService>();
    await footballService.GetTeamAsync(328);
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();
