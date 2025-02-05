using Core.Constants;
using Core.Entities;
using Core.Ultilities.Seeders;
using Infrastructure.Persistence.Seeders;

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
            await new JsonDataSeeder<Contact>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.ContactsFilePath).SeedAsync();
            await new JsonDataSeeder<News>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.NewsFilePath).SeedAsync();
            await new JsonDataSeeder<Department>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.DepartmentSeedPath).SeedAsync();
        }
    }
}