using Core.Entities;
using Core.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
               .HasMaxLength(255);

            builder.Property(n => n.Content)
                .IsRequired();

            builder.Property(n => n.ImageUrl)
                .HasMaxLength(500)
                .IsRequired(false)
                .HasDefaultValue("https://placehold.co/600x400");

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (NewsStatus)Enum.Parse(typeof(NewsStatus), s));

            builder.Property(c => c.NewsType)
                .HasConversion(
                s => s.ToString(),
                s => (NewsType)Enum.Parse(typeof(NewsType), s));
        }
    }
}
