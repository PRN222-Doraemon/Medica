using Core.Entities.Identity;
using MindSpace.Domain.Entities;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Resource : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set;} = DateTime.Now;
        public string Type { get; set; }
        public int CourseChapterID { get; set; }
        public CourseChapter CourseChapter { get; set; }
        public int CreatedByUserID { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }

    public enum ResourceType
    {
        [EnumMember(Value = "Lecture Slides")]
        Slide,
        [EnumMember(Value = "Online Video")]
        Video
    }
}
