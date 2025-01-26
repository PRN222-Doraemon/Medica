using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(c => c.User)
                   .WithMany()
                   .HasForeignKey(c => c.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Course)
                   .WithMany(course => course.Comments)
                   .HasForeignKey(c => c.CourseID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.SrcComment)
                   .WithMany(cc => cc.ReplyComments)
                   .HasForeignKey(c => c.SrcCommentID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Status)
                .HasConversion(
                c => c.ToString(),
                c => (CommentStatus)Enum.Parse(typeof(CommentStatus), c))
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(c => c.Title)
                   .HasColumnType("varchar(100)")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Details)
                   .IsRequired()
                   .HasColumnType("varchar(1000)")
                   .HasMaxLength(1000);
        }
    }
}