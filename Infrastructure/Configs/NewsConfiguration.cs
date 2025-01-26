using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            // Table
            builder.ToTable("News");

            // Properties
            builder.Property(n => n.Title)
                .IsRequired()
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

            builder.Property(n => n.Content)
                .IsRequired()
                .HasColumnType("varchar(max)");

            builder.Property(n => n.ImageUrl)
                .HasMaxLength(500)
                .HasColumnType("varchar(500)")
                .IsRequired(false)
                .HasDefaultValue("https://placehold.co/600x400");

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (NewsStatus)Enum.Parse(typeof(NewsStatus), s))
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            builder.Property(c => c.NewsType)
                .HasConversion(
                s => s.ToString(),
                s => (NewsType)Enum.Parse(typeof(NewsType), s))
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);
        }
    }
}
