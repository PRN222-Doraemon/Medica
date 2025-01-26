using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.Property(r => r.Title)
                   .IsRequired()
                   .HasColumnType("varchar(200)")
                   .HasMaxLength(200);

            builder.Property(r => r.Description)
                   .HasColumnType("varchar(1000)")
                   .HasMaxLength(1000);

            builder.Property(r => r.FileUrl)
                   .HasColumnType("varchar(500)")
                   .HasMaxLength(500);

            builder.Property(r => r.ResourceType)
                    .HasConversion(
                    r => r.ToString(),
                    r => (ResourceType)Enum.Parse(typeof(ResourceType), r ?? String.Empty))
                    .HasColumnType("varchar(50)");

            builder.HasOne(r => r.CourseChapter)
                   .WithMany(cc => cc.Resources)
                   .HasForeignKey(r => r.CourseChapterID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.CreatedBy)
                   .WithMany(e => e.Resources)
                   .HasForeignKey(r => r.CreatedByUserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}