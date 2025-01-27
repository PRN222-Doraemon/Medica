using Core.Entities.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeders;
using Infrastructure.Services.Ultilities.Seeders;
using Microsoft.AspNetCore.Identity;
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
                services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                // Add FileReader service
                services.AddScoped<IFileReader, FileReader>();

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
