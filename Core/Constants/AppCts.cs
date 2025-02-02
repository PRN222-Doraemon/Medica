using System.Reflection;

namespace Core.Constants
{
    public static class AppCts
    {
        public static readonly string AbsoluteProjectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        /// <summary>
        /// Location of Fake Json Filepath
        /// </summary>
        public static class SeederRelativePath
        {
            public static string CoursesFilePath = Path.Combine("Seeders", "CoursesSeedData.json");
        }
    }
}
