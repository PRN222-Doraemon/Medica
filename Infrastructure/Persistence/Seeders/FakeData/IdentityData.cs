﻿using Core.Constants;
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
                new(AppCts.Roles.Student)
                {
                    NormalizedName = AppCts.Roles.Student.ToUpper()
                },
                new(AppCts.Roles.Admin)
                {
                    NormalizedName = AppCts.Roles.Admin.ToUpper()
                },
                new(AppCts.Roles.Employee)
                {
                    NormalizedName = AppCts.Roles.Employee.ToUpper()
                },
                new(AppCts.Roles.Lecturer)
                {
                    NormalizedName = AppCts.Roles.Lecturer.ToUpper()
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
                    DateOfBirth = new DateTime(2000, 1, 1),
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    DateOfBirth = new DateTime(2000, 2, 2),
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    DateOfBirth = new DateTime(2000, 3, 3),
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    DateOfBirth = new DateTime(2000, 4, 4),
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    UpdatedAt = DateTime.UtcNow,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    DateOfBirth = new DateTime(1982, 2, 2),
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    DateOfBirth = new DateTime(1985, 3, 3),
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    UpdatedAt = DateTime.UtcNow,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                    UpdatedAt = DateTime.UtcNow,
                    ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTUE7jXHro7BdwC8kOvXnqQ7m1SwUP6MH6iK_ZLmKKjiG8tkhq7q_tytzMTXBtGsSRp0Y&usqp=CAU"
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
                        await userManager.AddToRoleAsync(user, AppCts.Roles.Student);
                        break;

                    case "employee":
                        await userManager.AddToRoleAsync(user, AppCts.Roles.Employee);
                        break;

                    case "admin":
                        await userManager.AddToRoleAsync(user, AppCts.Roles.Admin);
                        break;

                    case "lecturer":
                        await userManager.AddToRoleAsync(user, AppCts.Roles.Lecturer);
                        break;
                }
            }
        }
    }
}