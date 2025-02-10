using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Specifications.ClassSpecifications
{
    public class ClassroomWithCourseSpecification : BaseSpecification<Classroom>
    {
        public ClassroomWithCourseSpecification()
        {
            AddInclude(c => c.Course);
            AddInclude(c => c.Course.Category);
            AddInclude(c => c.Lecturer);
        }
    }
}