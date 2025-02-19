namespace MedicaWeb_MVC.ViewModels
{
    public class CourseChapterVM
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IEnumerable<ResourceVM> Resources { get; set; } = new List<ResourceVM>();
    }
}
