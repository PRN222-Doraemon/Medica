using Core.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MedicaWeb_MVC.ViewModels.News
{
    public class NewsUpsertVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        [ValidateNever]
        public string ImageUrl { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public NewsType NewsType { get; set; }
    }
}