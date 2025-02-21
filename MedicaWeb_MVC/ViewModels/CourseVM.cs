using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<CourseChapterVM> CourseChapters { get; set; } = new List<CourseChapterVM>();
        public IEnumerable<CommentVM> Comments { get; set; } = new List<CommentVM>();
        public IEnumerable<FeedbackVM> Feedbacks { get; set; } = new List<FeedbackVM>();
        public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();

        public int TotalChapters => CourseChapters.Count();
        public int TotalResources => CourseChapters.Sum(chapter => chapter.TotalResources);
        public decimal Rates => (Feedbacks.Count() > 0) ? Feedbacks.Average(f => f.Rating) : 0;
        public int Reviews => Feedbacks.Count();

    }
}
