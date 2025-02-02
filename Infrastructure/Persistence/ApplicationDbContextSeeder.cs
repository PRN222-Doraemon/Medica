using Core.Ultilities.Seeders;

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

        /// <summary>
        /// Add Json File here
        /// </summary>
        /// <returns></returns>
        public async Task SeedAllAsync()
        {
            //await new JsonDataSeeder<Restaurant>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.CoursesFilePath).SeedAsync();
        }
    }
}
