using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Resource : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public ResourceType ResourceType { get; set; }
        public int CourseChapterID { get; set; }
        public CourseChapter CourseChapter { get; set; }
        public int CreatedByUserID { get; set; }
        public Employee CreatedBy { get; set; }
    }

    public enum ResourceType
    {
        [EnumMember(Value = "Lecture Slides")]
        Slide,
        [EnumMember(Value = "Online Video")]
        Video
    }
}
