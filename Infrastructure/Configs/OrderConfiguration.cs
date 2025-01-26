using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.StudentId)
                .IsRequired();
            builder.Property(o => o.OrderTime)
                .IsRequired();
            builder.Property(o => o.Status)
                .HasConversion(s => s.ToString(),
                s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s));

            builder.Property(o => o.PaymentIntentId)
                .IsRequired();
            builder.Property(o => o.TotalPrice)
                .IsRequired();

            //1 Student - M Order
            builder.HasOne(s => s.Student)
                .WithMany(o => o.Orders)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
