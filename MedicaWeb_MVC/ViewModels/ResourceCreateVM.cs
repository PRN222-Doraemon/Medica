using Core.Entities.Identity;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class ResourceCreateVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public ResourceType ResourceType { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
