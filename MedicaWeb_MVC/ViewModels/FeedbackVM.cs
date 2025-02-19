using Core.Entities.Identity;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class FeedbackVM
    {
        public decimal Rating { get; set; }
        public string FeedbackContent { get; set; }
        public FeedbackStatus Status { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentImgUrl { get; set; } = string.Empty;
    }
}
