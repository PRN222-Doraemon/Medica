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
    }

    public enum ResourceType
    {
        [EnumMember(Value = "Slide")]
        Slide,
        [EnumMember(Value = "Video")]
        Video
    }
}
