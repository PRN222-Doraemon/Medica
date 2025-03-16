using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Orders
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId.Equals(paymentIntentId))
        {
            AddInclude(o => o.Student);
            AddInclude(o => o.OrderDetails);
        }

        public OrderSpecification(OrderParams orderParams)
            : base(o => (!orderParams.StudentID.HasValue || orderParams.StudentID == o.StudentId) && 
            (!orderParams.CourseID.HasValue || o.OrderDetails.Any(d => d.Classroom.CourseId == orderParams.CourseID)))
        {
            AddCustomInclude(o => o.Include(o => o.OrderDetails).ThenInclude(c => c.Classroom).ThenInclude(c => c.Course));
            AddCustomInclude(o => o.Include(o => o.OrderDetails).ThenInclude(c => c.Classroom).ThenInclude(c => c.Lecturer));
        }
        public OrderSpecification(DateTime startDate, DateTime endDate)
            : base(o => o.OrderTime >= startDate && o.OrderTime <= endDate)
        {
            AddInclude(o => o.OrderDetails);
            AddCustomInclude(o => o.Include(o => o.OrderDetails).ThenInclude(c => c.Classroom));
        }
    }
}
