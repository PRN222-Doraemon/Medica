using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            // Properties
            builder.Property(f => f.Rating)
             .HasColumnType("decimal(3, 2)")
             .IsRequired();

            builder.Property(f => f.FeedbackContent)
             .HasMaxLength(1000)
             .HasColumnType("varchar(1000)")
             .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (FeedbackStatus)Enum.Parse(typeof(FeedbackStatus), s))
                .HasColumnType("varchar(50)");

            // 1 Course - M Feedback
            builder.HasOne(c => c.Course)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // 1 Student - M Feedback
            builder.HasOne(c => c.Student)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}