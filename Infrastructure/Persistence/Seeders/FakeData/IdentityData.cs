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
                new(UserRoles.Student)
                {
                    NormalizedName = UserRoles.Student.ToUpper()
                },
                new(UserRoles.Admin)
                {
                    NormalizedName = UserRoles.Admin.ToUpper()
                },
                new(UserRoles.Employee)
                {
                    NormalizedName = UserRoles.Employee.ToUpper()
                },
                new(UserRoles.Lecturer)
                {
                    NormalizedName = UserRoles.Lecturer.ToUpper()
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
                new Student
                {
                    UserName = "student1",
                    NormalizedUserName = "STUDENT1",
                    Email = "student1@example.com",
                    NormalizedEmail = "STUDENT1@EXAMPLE.COM",
                    FirstName = "Student",
                    LastName = "One",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DateOfBirth = new DateTime(2000, 1, 1)
                },
                new Student
                {
                    UserName = "student2",
                    NormalizedUserName = "STUDENT2",
                    Email = "student2@example.com",
                    NormalizedEmail = "STUDENT2@EXAMPLE.COM",
                    FirstName = "Student",
                    LastName = "Two",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DateOfBirth = new DateTime(2000, 2, 2)
                },
                new Student
                {
                    UserName = "student3",
                    NormalizedUserName = "STUDENT3",
                    Email = "student3@example.com",
                    NormalizedEmail = "STUDENT3@EXAMPLE.COM",
                    FirstName = "Student",
                    LastName = "Three",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DateOfBirth = new DateTime(2000, 3, 3)
                },
                new Student
                {
                    UserName = "student4",
                    NormalizedUserName = "STUDENT4",
                    Email = "student4@example.com",
                    NormalizedEmail = "STUDENT4@EXAMPLE.COM",
                    FirstName = "Student",
                    LastName = "Four",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DateOfBirth = new DateTime(2000, 4, 4)
                },
                new ApplicationUser
                {
                    UserName = "admin1",
                    NormalizedUserName = "ADMIN1",
                    Email = "admin1@example.com",
                    NormalizedEmail = "ADMIN1@EXAMPLE.COM",
                    FirstName = "Admin",
                    LastName = "One",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Lecturer
                {
                    UserName = "lecturer1",
                    NormalizedUserName = "LECTURER1",
                    Email = "lecturer1@example.com",
                    NormalizedEmail = "LECTURER1@EXAMPLE.COM",
                    FirstName = "Lecturer",
                    LastName = "One",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DateOfBirth = new DateTime(1982, 2, 2)
                },
                new Lecturer
                {
                    UserName = "lecturer2",
                    NormalizedUserName = "LECTURER2",
                    Email = "lecturer2@example.com",
                    NormalizedEmail = "lecturer2@EXAMPLE.COM",
                    FirstName = "Lecturer",
                    LastName = "Two",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DateOfBirth = new DateTime(1985, 3, 3)
                },
                new Employee
                {
                    UserName = "employee1",
                    NormalizedUserName = "EMPLOYEE1",
                    Email = "employee1@example.com",
                    NormalizedEmail = "Manager1@EXAMPLE.COM",
                    FirstName = "Employee",
                    LastName = "One",
                    Status = UserStatus.Enabled,
                    CreatedAt = DateTime.UtcNow,
                    DepartmentId = 1,
                    UpdatedAt = DateTime.UtcNow
                },
                new Employee
                {
                    UserName = "employee2",
                    NormalizedUserName = "employee2",
                    Email = "employee2@example.com",
                    NormalizedEmail = "EMPLOYEE@EXAMPLE.COM",
                    FirstName = "Employee",
                    LastName = "Two",
                    Status = UserStatus.Enabled,
                    DepartmentId = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
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
                switch (user.FirstName.ToLower())
                {
                    case "student":
                        await userManager.AddToRoleAsync(user, UserRoles.Student);
                        break;

                    case "employee":
                        await userManager.AddToRoleAsync(user, UserRoles.Employee);
                        break;

                    case "admin":
                        await userManager.AddToRoleAsync(user, UserRoles.Admin);
                        break;

                    case "lecturer":
                        await userManager.AddToRoleAsync(user, UserRoles.Lecturer);
                        break;
                }
            }
        }
    }
}