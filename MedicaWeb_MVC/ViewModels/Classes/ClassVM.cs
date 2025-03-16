using Core.Entities;
using MedicaWeb_MVC.ViewModels.Courses;
using MedicaWeb_MVC.ViewModels.User;

namespace MedicaWeb_MVC.ViewModels.Classes
{
    public class ClassVM
    {
        public int Id { get; set; }
        public ClassroomMode Mode { get; set; }
        public string? Description { get; set; }
        public int MaxParticipant { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CourseVM Course { get; set; }
        public LecturerVM Lecturer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public IEnumerable<StudentVM> Students { get; set; } = new List<StudentVM>();
        public int TotalEnrolls => OrderDetails.Count();
        public ClassroomStatus Status { get; set; }

        public ClassroomStatus ComputedStatus
        {
            get
            {
                if (Status == ClassroomStatus.Cancelled)
                    return Status;
                var today = DateOnly.FromDateTime(DateTime.Today);
                if (StartDate > today) return ClassroomStatus.Upcoming;
                if (EndDate <= today) return ClassroomStatus.Completed;
                return ClassroomStatus.Ongoing;
            }
        }
    }
}

