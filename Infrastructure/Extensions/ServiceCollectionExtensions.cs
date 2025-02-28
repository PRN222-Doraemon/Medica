using Core.Entities.Identity;
using Core.Ultilities.Seeders;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                // Registers the database context with the DI container
                services.AddDbContext<ApplicationDbContext>(opt =>
                {
                    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });

                // Register the Redis with IDistributed API
                services.AddStackExchangeRedisCache(opt =>
                {
                    opt.Configuration = configuration["Redis:ConnectionString"];
                });

                // Configure Identity and Authentication & Authorization
                services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequiredLength = 8;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();

                services.ConfigureApplicationCookie(o =>
                {
                    o.Cookie.HttpOnly = true;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.Cookie.SameSite = SameSiteMode.Strict;
                    o.Cookie.IsEssential = true;

                    o.LoginPath = "/Accounts/Login";
                    o.LogoutPath = "/Home/Index";
                    o.AccessDeniedPath = "/Accounts/AccessDenied";
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Non-Activiy time is 30 minutes
                    o.SlidingExpiration = true; // Extend the ticket if active
                });

                // Add FileReader service
                services.AddScoped<IFileReader, FileReader>();

                // Set Service for Identity Seeders
                services.AddScoped<ApplicationIdentityDbContextSeeder>();

                // Set Services for Data Seeders
                services.AddScoped<ApplicationDbContextSeeder>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}