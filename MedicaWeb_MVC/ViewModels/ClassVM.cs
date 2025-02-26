using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class ClassVM
    {
        public int Id { get; set; }
        public ClassroomMode Mode { get; set; }
        public ClassroomStatus Status { get; set; }
        public string? Description { get; set; }
        public int MaxParticipant {  get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CourseName { get; set; }
        public string CourseId { get; set; }
        public string CourseImgUrl { get; set; }
        public string LecturerName { get; set; }
    }
}
