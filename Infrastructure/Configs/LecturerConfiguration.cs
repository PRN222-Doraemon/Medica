using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class LecturerConfiguration : IEntityTypeConfiguration<Lecturer>
    {
        public void Configure(EntityTypeBuilder<Lecturer> builder)
        {
            // Table Name
            builder.ToTable("Lecturers").HasBaseType<ApplicationUser>();

            // 1 Lecturer - 1 User
            builder.HasOne(s => s.User)
                .WithOne(u => u.Lecturer)
                .HasForeignKey<Lecturer>(s => s.Id);
        }
    }
}
