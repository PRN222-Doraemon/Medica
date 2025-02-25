using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Classes
{
    public class ClassSpecification : BaseSpecification<Classroom>
    {
        public ClassSpecification(ClassParams classParams) :
            base(x => (string.IsNullOrEmpty(classParams.Keyword) || x.Course.Name.Contains(classParams.Keyword)) &&
            (!classParams.CategoryId.HasValue || classParams.CategoryId == x.Course.Category.Id) &&
            (!classParams.ClassroomStatus.HasValue || classParams.ClassroomStatus == x.Status))
        {
            AddCustomInclude(c => c.Include(c => c.Course).ThenInclude(c => c.Category));
            AddInclude(c => c.Lecturer);
            switch (classParams.SortOrder)
            {
                case "newest":
                    AddOrderBy(c => c.UpdatedAt);
                    break;
                case "oldest":
                    AddOrderByDescending(c => c.UpdatedAt);
                    break;
                case "name":
                    AddOrderBy(c => c.Course.Name);
                    break;
                case "nameDesc":
                    AddOrderByDescending(c => c.Course.Name);
                    break;
            }
        }
    }
}
