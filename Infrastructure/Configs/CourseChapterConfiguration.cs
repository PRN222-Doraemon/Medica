using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class CourseChapterConfiguration : IEntityTypeConfiguration<CourseChapter>
    {
        public void Configure(EntityTypeBuilder<CourseChapter> builder)
        {
            builder.Property(cc => cc.Name)
                   .IsRequired()
                   .HasColumnType("varchar(200)")
                   .HasMaxLength(200);

            builder.Property(cc => cc.Description)
                   .HasColumnType("varchar(1000)")
                   .HasMaxLength(1000);

            builder.HasOne(cc => cc.Course)
                   .WithMany(course => course.CourseChapters)
                   .HasForeignKey(cc => cc.CourseID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}