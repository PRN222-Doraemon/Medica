using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.News
{
    public class NewsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public NewsType NewsType { get; set; }
        public NewsStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}