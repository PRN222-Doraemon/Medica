using Infrastructure.Services.Ultilities.Seeders;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextSeeder
    {
        // =====================================
        // === Props & Fields
        // =====================================

        private readonly IDataSeeder _dataSeeder;
        private readonly ApplicationDbContext _dbContext;

        // =====================================
        // === Constructors
        // =====================================

        public ApplicationDbContextSeeder(ApplicationDbContext dbContext, IDataSeeder dataSeeder)
        {
            _dbContext = dbContext;
            _dataSeeder = dataSeeder;
        }

        // =====================================
        // === Methods
        // =====================================

        public async Task SeedAllAsync()
        {
            await _dataSeeder.SeedAsync();
        }
    }
}
