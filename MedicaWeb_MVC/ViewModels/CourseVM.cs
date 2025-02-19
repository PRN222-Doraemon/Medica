using Core.Entities.Identity;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class CourseVM
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CreatedByUserName { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }
        public TimeSpan Duration { get; set; }
        public IEnumerable<CourseChapterVM> CourseChapters { get; set; } = new List<CourseChapterVM>();
        public IEnumerable<CommentVM> Comments { get; set; } = new List<CommentVM>();

        // 1 Course - M Classroom
        public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();
    }
}
