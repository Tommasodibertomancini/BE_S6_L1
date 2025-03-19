using BE_S6_L1.Data;
using BE_S6_L1.Models;
using BE_S6_L1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LoggingService.ConfigureLogging(builder.Configuration);

try
{
    Log.Information("Avvio applicazione Studenti");

    builder.Host.UseSerilog();

    // Aggiungere servizi al container
    builder.Services.AddControllersWithViews();

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

    // Configurare le opzioni dei cookie di Identity
    builder.Services.ConfigureApplicationCookie(options => {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

    // Configurare il DbContext
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    var app = builder.Build();

    // Configurare la pipeline HTTP
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Studenti}/{action=Index}/{id?}");

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Assicura che il database esista
            context.Database.Migrate();

            // Inizializza i ruoli e gli utenti admin se necessario
            // InitializeRolesAndUsers(userManager, roleManager).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Si è verificato un errore durante l'inizializzazione del database.");
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "L'applicazione si è arrestata in modo imprevisto");
}
finally
{
    Log.CloseAndFlush();
}