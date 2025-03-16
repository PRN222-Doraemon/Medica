using Core.Entities.Identity;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Courses
{
    public class FeedbackUpsertVM
    {
        public int Id { get; set; }
        public decimal Rating { get; set; }
        public string FeedbackContent { get; set; }

        public int CourseId { get; set; }
    }
}
