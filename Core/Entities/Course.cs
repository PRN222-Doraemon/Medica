using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = CourseStatus.Active.ToString();
        public TimeSpan Duration { get; set; }
        public IEnumerable<CourseChapter> CourseChapters { get; set; } = new List<CourseChapter>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

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
