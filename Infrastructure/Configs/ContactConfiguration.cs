using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            // Table Name
            builder.ToTable("Contacts");

            // Properties
            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(c => c.LastContacted)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(c => c.IsSubscribed)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (ContactStatus)Enum.Parse(typeof(ContactStatus), s))
                .HasColumnType("varchar(50)");

            // 1 Contact - Owns 1 Address
            builder.OwnsOne(c => c.Address, a =>
            {
                a.Property(a => a.Street).HasColumnType("varchar(100)").HasMaxLength(100);
                a.Property(a => a.City).HasColumnType("varchar(50)").HasMaxLength(50);
                a.Property(a => a.Ward).HasColumnType("varchar(50)").HasMaxLength(50);
                a.Property(a => a.Province).HasColumnType("varchar(50)").HasMaxLength(50);
                a.Property(a => a.PostalCode).HasColumnType("varchar(10)").HasMaxLength(10);
            });
        }
    }
}