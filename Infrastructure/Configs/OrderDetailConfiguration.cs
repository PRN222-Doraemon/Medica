using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(o => o.OrderID)
                .IsRequired();
            builder.Property(o => o.ClassroomID)
                .IsRequired();
            builder.Property(o => o.Price)
                .IsRequired();

            //1 Order - M OrderDetails
            builder.HasOne(o => o.Order)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(o => o.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
            //1 Order - M OrderDetails
            builder.HasOne(c => c.Classroom)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(c => c.ClassroomID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
