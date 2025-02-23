using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Orders
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(OrderParams orderParams)
            :base (o => (!orderParams.StudentID.HasValue || orderParams.StudentID == o.StudentId) )
        {
            AddCustomInclude(o => o.Include(o => o.OrderDetails).ThenInclude(c => c.Classroom).ThenInclude(c => c.Course));
            AddCustomInclude(o => o.Include(o => o.OrderDetails).ThenInclude(c => c.Classroom).ThenInclude(c => c.Lecturer));
        }
    }
}
