using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public int CreatedByUserID { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public Category Category { get; set; }
        public CourseStatus Status { get; set; }
        public TimeSpan Duration { get; set; }
        public IEnumerable<CourseChapter> CourseChapters { get; set; } = new List<CourseChapter>();
        public IEnumerable<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        // 1 Course - M Classroom
        public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();
    }

    public enum CourseStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Inactive")]
        Inactive,
    }
}
