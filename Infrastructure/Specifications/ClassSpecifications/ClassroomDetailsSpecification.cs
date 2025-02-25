using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Specifications.ClassSpecifications
{
    public class ClassroomDetailsSpecification : BaseSpecification<Classroom>
    {
        public ClassroomDetailsSpecification(int id) : base(c => c.Id == id)
        {
            AddInclude(c => c.Course);
            AddInclude(c => c.Lecturer);
        }
    }
}