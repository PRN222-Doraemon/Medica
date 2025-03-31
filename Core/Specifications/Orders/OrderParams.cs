using Core.Entities;

namespace Core.Specifications.Orders
{
    public class OrderParams : PagingParams
    {
        public int? StudentID { get; set; }
        public int? CourseID { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Search { get; set; }
    }
}
