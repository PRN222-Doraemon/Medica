using Core.Entities;

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
