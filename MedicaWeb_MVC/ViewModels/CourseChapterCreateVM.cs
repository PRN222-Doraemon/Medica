namespace MedicaWeb_MVC.ViewModels
{
    public class CourseChapterCreateVM
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<ResourceCreateVM> Resources { get; set; } = new List<ResourceCreateVM>();
    }
}
