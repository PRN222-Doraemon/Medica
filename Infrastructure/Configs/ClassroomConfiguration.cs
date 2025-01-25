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
    public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            // Properties
            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.MaxParticipant).IsRequired();
            builder.Property(c => c.Price).HasColumnType("decimal(18,4)");

            builder.Property(c => c.Mode)
                .HasConversion(
                s => s.ToString(),
                s => (ClassroomMode)Enum.Parse(typeof(ClassroomMode), s));

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (ClassroomStatus)Enum.Parse(typeof(ClassroomStatus), s));

            // 1 Course - M Classroom
            builder.HasOne(c => c.Course)
                .WithMany(l => l.Classrooms)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // 1 Lecturer - M Classroom
            builder.HasOne(c => c.Lecturer)
                .WithMany(l => l.Classrooms)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
