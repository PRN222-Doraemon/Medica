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
            public static string DepartmentSeedPath = Path.Combine(JsonPath, "Department.json");
            public static string ContactsFilePath = Path.Combine(JsonPath, "ContactsData.json");
            public static string NewsFilePath = Path.Combine(JsonPath, "NewsData.json");
        }
    }
}
