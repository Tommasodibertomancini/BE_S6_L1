using BE_S6_L1.Data;
using BE_S6_L1.Services;
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