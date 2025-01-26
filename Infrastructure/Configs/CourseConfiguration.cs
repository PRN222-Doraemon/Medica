using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasColumnType("varchar(200)")
                   .HasMaxLength(200);

            builder.Property(c => c.Description)
                   .HasColumnType("varchar(1000)")
                   .HasMaxLength(1000);

            builder.Property(c => c.ImgUrl)
                   .HasColumnType("varchar(500)")
                   .HasMaxLength(500);

            builder.HasOne(c => c.Category)
                   .WithMany(cat => cat.Courses)
                   .HasForeignKey(c => c.CategoryID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.Status)
                    .HasConversion(
                    c => c.ToString(),
                    c => (CourseStatus)Enum.Parse(typeof(CourseStatus), c))
                    .HasColumnType("varchar(50)");

            builder.Property(c => c.Duration)
                   .IsRequired();
        }
    }
}