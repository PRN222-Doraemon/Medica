using Core.Entities;
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
             .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (FeedbackStatus)Enum.Parse(typeof(FeedbackStatus), s));

            // 1 Lecturer - M Feedback
            builder.HasOne(c => c.Lecturer)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // 1 Classroom - M Feedback
            builder.HasOne(c => c.Classroom)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(c => c.ClassroomId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // 1 Student - M Feedback
            builder.HasOne(c => c.Student)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
