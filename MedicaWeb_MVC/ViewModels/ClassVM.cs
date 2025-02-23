using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class ClassVM
    {
        public ClassroomMode Mode { get; set; }
        public ClassroomStatus Status { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string CourseName { get; set; }
        public string CourseId { get; set; }
        public string CourseImgUrl { get; set; }
        public string LecturerName { get; set; }
    }
}
