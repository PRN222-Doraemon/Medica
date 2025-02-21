using Core.Entities.Identity;
using Core.Ultilities.Seeders;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

                // Using Cookie-based Authorization and Google Authentication
                services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                }).AddCookie(o =>
                {
                    o.Cookie.HttpOnly = true;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.Cookie.SameSite = SameSiteMode.Strict;
                    o.LoginPath = "/Account/Login";
                    o.AccessDeniedPath = "/Account/AccessDenied";
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expire within 30m
                    o.SlidingExpiration = true; // Extend the cookie if active
                }).AddGoogle(o =>
                {
                    o.ClientId = configuration["Authentication:Google:ClientId"];
                    o.ClientSecret = configuration["Authentication:Google:ClientSecret"];
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