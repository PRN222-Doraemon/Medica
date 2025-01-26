using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees").HasBaseType<ApplicationUser>();

            // 1 Employee - M Departments
            builder
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            // 1 Employee - 1 Application User
            builder
                .HasOne(e => e.User)
                .WithOne(au => au.Employee)
                .HasForeignKey<Employee>(e => e.Id);
        }
    }
}