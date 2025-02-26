﻿using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class ClassVM
    {
        public int Id { get; set; }
        public ClassroomMode Mode { get; set; }
        public string? Description { get; set; }
        public int MaxParticipant {  get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CourseVM Course { get; set; }
        public int LecturerId { get; set; }
        public string LecturerName { get; set; }
        public string LecturerImgUrl { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
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
