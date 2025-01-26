using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            // Properties
            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate).IsRequired();
            builder.Property(c => c.Description)
                .HasColumnType("varchar(500)").HasMaxLength(500);
            builder.Property(c => c.MaxParticipant).IsRequired();
            builder.Property(c => c.Price).HasColumnType("decimal(18,4)");

            builder.Property(c => c.Mode)
                .HasConversion(
                s => s.ToString(),
                s => (ClassroomMode)Enum.Parse(typeof(ClassroomMode), s))
                .HasColumnType("varchar(50)");

            builder.Property(c => c.Status)
                .HasConversion(
                s => s.ToString(),
                s => (ClassroomStatus)Enum.Parse(typeof(ClassroomStatus), s))
                .HasColumnType("varchar(50)");

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
