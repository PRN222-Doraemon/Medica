using Core.Entities.Identity;
using Infrastructure.Persistence.Seeders.FakeData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationIdentityDbContextSeeder
    {
        // =====================================
        // === Props & Fields
        // =====================================

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string PASSWORD = "Password!";

        // =====================================
        // === Constructors
        // =====================================

        public ApplicationIdentityDbContextSeeder(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // =====================================
        // === Methods
        // =====================================

        /// <summary>
        /// Seed Identity
        /// </summary>
        /// <returns></returns>
        public async Task SeedIdentityAsync()
        {
            IEnumerable<ApplicationUser> users = IdentityData.GetUsers();
            if (await _dbContext.Database.CanConnectAsync())
            {
                // Seed Roles
                if (!await _dbContext.Roles.AnyAsync())
                {
                    await _dbContext.AddRangeAsync(IdentityData.GetRoles());
                    await _dbContext.SaveChangesAsync();
                }

                // Seed Users with Password
                //if (!await _dbContext.Users.AnyAsync())
                //{
                //    foreach (ApplicationUser user in users)
                //    {
                //        await _userManager.CreateAsync(user, PASSWORD);
                //    }
                //}

                // Seed Users with Roles
                //if (!users.IsNullOrEmpty() && await _dbContext.Roles.AnyAsync())
                //{
                //    await IdentityData.GetUserRoles(users, _userManager);
                //}
            }
        }


    }
}
