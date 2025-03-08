using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Feedback : BaseEntity
    {
        public decimal Rating { get; set; }
        public string FeedbackContent { get; set; }
        public FeedbackStatus Status { get; set; } = FeedbackStatus.Enabled;


        // 1 Student - M Feedback
        public int StudentId { get; set; }
        public Student Student { get; set; }


        // 1 Course - M Feedback
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }

    public enum FeedbackStatus
    {
        [EnumMember(Value = "Enabled")]
        Enabled,
        [EnumMember(Value = "Disabled")]
        Disabled
    }
}
