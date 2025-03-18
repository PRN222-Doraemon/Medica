using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Courses
{
    public class ResourceCreateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public IFormFile? File { get; set; }
        public ResourceType ResourceType { get; set; }
    }
}
