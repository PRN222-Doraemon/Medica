using Infrastructure.Services.Ultilities.Seeders;
using System.Runtime.Serialization.Formatters;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextSeeder
    {
        // =====================================
        // === Props & Fields
        // =====================================

        private readonly IFileReader _fileReader;

        private readonly ApplicationDbContext _dbContext;

        // =====================================
        // === Constructors
        // =====================================

        public ApplicationDbContextSeeder(ApplicationDbContext dbContext, IFileReader fileReader)
        {
            _dbContext = dbContext;
            _fileReader = fileReader;
        }

        // =====================================
        // === Methods
        // =====================================

        public async Task SeedAllAsync()
        {
            //await _dataSeeder.SeedAsync();
        }
    }
}
