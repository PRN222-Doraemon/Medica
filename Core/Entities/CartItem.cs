namespace Core.Entities
{
    public class CartItem
    {
        public int ClassRoomId { get; set; }
        public int CourseId { get; set; }
        public string ImageUrl { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
