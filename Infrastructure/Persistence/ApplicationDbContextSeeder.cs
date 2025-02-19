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

        private readonly ApplicationIdentityDbContextSeeder _dbContextIdentitySeeder;


        // =====================================
        // === Constructors
        // =====================================

        public ApplicationDbContextSeeder(ApplicationDbContext dbContext, IFileReader fileReader, ApplicationIdentityDbContextSeeder dbContextIdentitySeeder)
        {
            _dbContext = dbContext;
            _fileReader = fileReader;
            _dbContextIdentitySeeder = dbContextIdentitySeeder;
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
            // Identity & Departments need to seed first 
            await new JsonDataSeeder<Department>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.DepartmentPath).SeedAsync();
            await _dbContextIdentitySeeder.SeedIdentityAsync();

            await new JsonDataSeeder<Contact>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.ContactsPath).SeedAsync();
            await new JsonDataSeeder<News>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.NewsPath).SeedAsync();
            await new JsonDataSeeder<Category>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.CategoryPath).SeedAsync();
            await new JsonDataSeeder<Course>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.CoursePath).SeedAsync();
            await new JsonDataSeeder<Classroom>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.ClassroomPath).SeedAsync();            
            await new JsonDataSeeder<CourseChapter>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.CourseChapterPath).SeedAsync();
            await new JsonDataSeeder<Resource>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.ResourcePath).SeedAsync();
            await new JsonDataSeeder<Comment>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.CommentPath).SeedAsync();
            await new JsonDataSeeder<Feedback>(_fileReader, _dbContext).AddRelativeFilePath(AppCts.SeederRelativePath.FeedbackPath).SeedAsync();
        }
    }
}