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
                .HasMaxLength(255);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);

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
                s => (ContactStatus)Enum.Parse(typeof(ContactStatus), s));

            // 1 Contact - Owns 1 Address
            builder.OwnsOne(c => c.Address, a =>
            {
                a.Property(a => a.Street).IsUnicode().HasMaxLength(100);
                a.Property(a => a.City).IsUnicode().HasMaxLength(50);
                a.Property(a => a.Ward).IsUnicode().HasMaxLength(50);
                a.Property(a => a.Province).IsUnicode().HasMaxLength(50);
                a.Property(a => a.PostalCode).HasMaxLength(10);
            });
        }
    }
}
