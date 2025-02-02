using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Seeders.FakeData
{
    public static class IdentityData
    {
        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApplicationRole> GetRoles()

        {
            List<ApplicationRole> roles = new List<ApplicationRole>
            {
                new ApplicationRole(UserRoles.Admin)
                {
                    NormalizedName =UserRoles.Admin.ToUpper(),
                },

                new ApplicationRole(UserRoles.Student)
                {
                    NormalizedName =UserRoles.Student.ToUpper(),
                },

                new ApplicationRole(UserRoles.Employee)
                {
                    NormalizedName =UserRoles.Employee.ToUpper(),
                },
            };

            return roles;
        }


        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApplicationUser> GetUsers()
        {
            List<ApplicationUser> users = new List<ApplicationUser>
            {
                // FILLING THE OBJECT HERE
            };

            return users;
        }


        /// <summary>
        /// Assign Role to Users
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static async Task GetUserRoles(IEnumerable<ApplicationUser> users, UserManager<ApplicationUser> userManager)
        {
            foreach (var user in users)
            {
                switch (user.UserName.ToLower())
                {
                    case "admin":
                        await userManager.AddToRoleAsync(user, UserRoles.Admin);
                        break;

                    case "user":
                        await userManager.AddToRoleAsync(user, UserRoles.Lecturer);
                        break;
                    case "owner":
                        await userManager.AddToRoleAsync(user, UserRoles.Employee);
                        break;
                }
            }
        }
    }
}
