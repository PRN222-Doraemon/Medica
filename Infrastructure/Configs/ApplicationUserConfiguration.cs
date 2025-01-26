using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configs
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(au => au.Email).IsUnique();
            builder.HasIndex(au => au.PhoneNumber).IsUnique();
            builder.HasIndex(au => au.UserName).IsUnique();

            // Add varchar specifications for string properties
            builder.Property(au => au.Email).HasColumnType("varchar(256)").HasMaxLength(256);
            builder.Property(au => au.PhoneNumber).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.Property(au => au.UserName).HasColumnType("varchar(256)").HasMaxLength(256);
            builder.Property(au => au.FirstName).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(au => au.LastName).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(au => au.ImageUrl).HasColumnType("varchar(500)").HasMaxLength(500);
        }
    }
}
