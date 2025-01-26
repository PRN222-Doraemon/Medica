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

        internal DbSet<Lecturer> Lecturers { get; set; }
        internal DbSet<Student> Students { get; set; }
        internal DbSet<Employee> Employees { get; set; }
        internal DbSet<Department> Departments { get; set; }

        internal DbSet<Course> Courses { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<CourseChapter> CourseChapters { get; set; }
        internal DbSet<Classroom> Classrooms { get; set; }
        internal DbSet<Comment> Comments { get; set; }
        internal DbSet<Feedback> Feedbacks { get; set; }

        internal DbSet<Order> Orders { get; set; }
        internal DbSet<OrderDetail> OrderDetails { get; set; }

        internal DbSet<Resource> Resources { get; set; }
        internal DbSet<Contact> Contacts { get; set; }
        internal DbSet<News> News { get; set; }

        // ========================
        // === Constructors
        // ========================

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

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
                        .Property(nameof(BaseEntity.CreatedAt))
                        .IsRequired()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAdd();

                    // Configure UpdatedAt
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.UpdatedAt))
                        .IsRequired()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAddOrUpdate();
                }
            }
        }
    }
}