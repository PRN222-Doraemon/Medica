using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Classroom : BaseEntity
    {
        public ClassroomMode Mode { get; set; }
        public ClassroomStatus Status { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public string Description { get; set; }
        public int MaxParticipant { get; set; }

        // 1 course - M classroom 
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // 1 lecturer - M classroom
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }

        // 1 Classrom - M OrderDetails
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();

    }

    public enum ClassroomMode
    {
        [EnumMember(Value = "Online")]
        Online,

        [EnumMember(Value = "Offline")]
        Offline,
    }

    public enum ClassroomStatus
    {
        [EnumMember(Value = "Active")]
        Active,

        [EnumMember(Value = "Cancelled")]
        Cancelled
    }
}
