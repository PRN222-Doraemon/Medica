using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(d => d.Name)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);
            builder.Property(d => d.Description)
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);
        }
    }
}
