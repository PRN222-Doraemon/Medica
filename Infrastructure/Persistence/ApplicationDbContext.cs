using Core.Entities;

using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        // ========================
        // === DbSets
        // ========================

        private DbSet<Contact> Contacts { get; set; }
        private DbSet<Lecturer> Lecturers { get; set; }
        private DbSet<Student> Students { get; set; }
        private DbSet<News> News { get; set; }
        private DbSet<Classroom> Classrooms { get; set; }

        // ========================
        // === Constructors
        // ========================

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseChapter> CourseChapters { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        // ========================
        // === Methods
        // ========================


        /// <summary>
        /// Override the Model Creating 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name)
                      .HasColumnType("varchar(100)")
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasColumnType("varchar(200)")
                      .HasMaxLength(200);

                entity.Property(c => c.Description)
                      .HasColumnType("varchar(1000)")
                      .HasMaxLength(1000);

                entity.Property(c => c.ImgUrl)
                      .HasColumnType("varchar(500)")
                      .HasMaxLength(500);

                entity.HasOne(c => c.Category)
                      .WithMany(cat => cat.Courses)
                      .HasForeignKey(c => c.CategoryID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(c => c.Status)
                      .IsRequired()
                      .HasColumnType("varchar(20)")
                      .HasMaxLength(20);

                entity.Property(c => c.Duration)
                      .IsRequired();
            });

            // Configure CourseChapter entity
            modelBuilder.Entity<CourseChapter>(entity =>
            {
                entity.Property(cc => cc.Name)
                      .IsRequired()
                      .HasColumnType("varchar(200)")
                      .HasMaxLength(200);

                entity.Property(cc => cc.Description)
                      .HasColumnType("varchar(1000)")
                      .HasMaxLength(1000);

                entity.HasOne(cc => cc.Course)
                      .WithMany(course => course.CourseChapters)
                      .HasForeignKey(cc => cc.CourseID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Resource entity
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(r => r.Title)
                      .IsRequired()
                      .HasColumnType("varchar(200)")
                      .HasMaxLength(200);

                entity.Property(r => r.Description)
                      .HasColumnType("varchar(1000)")
                      .HasMaxLength(1000);

                entity.Property(r => r.FileUrl)
                      .HasColumnType("varchar(500)")
                      .HasMaxLength(500);

                entity.Property(r => r.Type)
                      .IsRequired()
                      .HasColumnType("varchar(100)")
                      .HasMaxLength(100);

                entity.HasOne(r => r.CourseChapter)
                      .WithMany(cc => cc.Resources)
                      .HasForeignKey(r => r.CourseChapterID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.CreatedBy)
                      .WithMany()
                      .HasForeignKey(r => r.CreatedByUserID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Course)
                      .WithMany(course => course.Comments)
                      .HasForeignKey(c => c.CourseID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.SrcComment)
                      .WithMany(cc => cc.ReplyComments)
                      .HasForeignKey(c => c.SrcCommentID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(c => c.Title)
                      .HasColumnType("varchar(100)")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Details)
                      .IsRequired()
                      .HasColumnType("varchar(1000)")
                      .HasMaxLength(1000);

                entity.Property(c => c.Status)
                      .IsRequired()
                      .HasColumnType("varchar(20)")
                      .HasMaxLength(20);
            });

            // Apply all configurations in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Apply base entity to all inheriting entities
            ApplyBaseEntityToDerivedClass(modelBuilder);
        }

        /// <summary>
        /// Configuring the base entity
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void ApplyBaseEntityToDerivedClass(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(BaseEntity))
                {
                    // Configure Id
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.Id))
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();

                    // Configure CreatedAt
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.CreateAt))
                        .IsRequired()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAdd();

                    // Configure UpdatedAt
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.UpdateAt))
                        .IsRequired()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAddOrUpdate();
                }
            }
        }
    }
}