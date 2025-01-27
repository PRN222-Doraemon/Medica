namespace Core.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int ClassroomID { get; set; }
        public Classroom Classroom { get; set; }
        public decimal Price { get; set; }


    }
}
