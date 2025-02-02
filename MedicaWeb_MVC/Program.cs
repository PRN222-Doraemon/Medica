using Infrastructure.Extensions;
using Infrastructure.Persistence;
using MedicaWeb_MVC.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====================================
// === Add services to the container
// ====================================

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// ====================================
// === Build the application
// ====================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// ====================================
// === Use Middlewares
// ====================================

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ===================================================
// === Create a scope and call the service manually
// ===================================================

if (builder.Configuration.GetValue<bool>("IsSeedingMode"))
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var applicationSeeder = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeder>();
    var identitySeeder = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContextSeeder>();
    using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        await applicationSeeder.SeedAllAsync();
        await identitySeeder.SeedIdentityAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error happens during migrations!");
    }
}
app.Run();