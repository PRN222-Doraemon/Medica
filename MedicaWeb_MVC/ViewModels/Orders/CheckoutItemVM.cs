namespace MedicaWeb_MVC.ViewModels.Orders
{
    public class CheckoutItemVM
    {
        public int ClassRoomId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string ClassRoomDescription { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public TimeSpan CourseDuration { get; set; }
        public decimal Price { get; set; }
    }
}
