using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Orders
{
    public class CartItemVM
    {
        public int ClassRoomId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public int Quantity { get; set; } = 1;
        public string? InstructorName { get; set; }
        public DateOnly StartDate { get; set; }
        public TimeSpan Duration { get; set; }
        public ClassroomMode Mode { get; set; }
        public string? Description { get; set; }
    }
}
