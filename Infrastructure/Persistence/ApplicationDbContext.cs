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