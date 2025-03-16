using Core.Entities;
using MedicaWeb_MVC.ViewModels.Classes;

namespace MedicaWeb_MVC.ViewModels.Courses
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<CourseChapterVM> CourseChapters { get; set; } = new List<CourseChapterVM>();
        public virtual IEnumerable<FeedbackVM> Feedbacks { get; set; } = new List<FeedbackVM>();
        public virtual ICollection<ClassVM> Classrooms { get; set; } = new HashSet<ClassVM>();

        public int TotalChapters => CourseChapters.Count();
        public int TotalResources => CourseChapters.Sum(chapter => chapter.TotalResources);
        public decimal Rates => Feedbacks.Count() > 0 ? Math.Round(Feedbacks.Average(f => f.Rating), 2) : 0;

        public int Reviews => Feedbacks.Count();
        public int TotalEnrolls => Classrooms.SelectMany(c => c.OrderDetails).Count();

    }
}
