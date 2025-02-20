using Core.Entities.Identity;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class ResourceCreateVM
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public ResourceType ResourceType { get; set; }
        public int CreatedByUserID { get; set; } = 1;
    }
}
