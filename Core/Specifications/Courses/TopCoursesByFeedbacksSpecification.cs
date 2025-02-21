using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Courses
{
    public class TopCoursesByFeedbacksSpecification : BaseSpecification<Course>
    {
        public TopCoursesByFeedbacksSpecification(int count)
        {
            AddInclude(x => x.Feedbacks);
            AddOrderByDescending(x => x.Feedbacks.Count());
            ApplyPaging(0, count);
        }
    }
}
