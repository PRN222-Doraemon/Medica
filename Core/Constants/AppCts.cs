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
            public static string JsonPath = Path.Combine("Persistence", "Seeders", "FakeData");
            public static string DepartmentPath = Path.Combine(JsonPath, "Department.json");
            public static string ContactsPath = Path.Combine(JsonPath, "ContactsData.json");
            public static string NewsPath = Path.Combine(JsonPath, "NewsData.json");
            public static string CategoryPath = Path.Combine(JsonPath, "CategoryData.json");
            public static string CoursePath = Path.Combine(JsonPath, "CourseData.json");
            public static string ClassroomPath = Path.Combine(JsonPath, "ClassroomData.json");
            public static string FeedbackPath = Path.Combine(JsonPath, "FeedbackData.json");
            public static string CourseChapterPath = Path.Combine(JsonPath, "CourseChapterData.json");
            public static string ResourcePath = Path.Combine(JsonPath, "ResourceData.json");
            public static string CommentPath = Path.Combine(JsonPath, "CommentData.json");
        }
        public static class Display
        {
            public const int MAX_VISIBLE_REVIEWS = 2;
            public const int MAX_VISIBLE_CHAPTERS = 3;
            public const int MAX_VISIBLE_RESOURCES = 10;
        }
    }
}
