using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Classes
{
    public class ClassSpecification : BaseSpecification<Classroom>
    {
        public ClassSpecification(ClassParams classParams, bool applyPaging = true) :
            base(x => (string.IsNullOrEmpty(classParams.Search) || x.Course.Name.ToLower().Contains(classParams.Search)) &&
            (!classParams.CategoryId.HasValue || classParams.CategoryId == x.Course.Category.Id) &&
            (!classParams.CourseId.HasValue || classParams.CourseId == x.Course.Id) &&
            (!classParams.ClassroomStatus.HasValue || (x.Status == ClassroomStatus.Active &&
            ((classParams.ClassroomStatus == ClassroomStatus.Upcoming && x.StartDate > DateOnly.FromDateTime(DateTime.Today)) ||
            (classParams.ClassroomStatus == ClassroomStatus.Completed && x.EndDate < DateOnly.FromDateTime(DateTime.Today)) ||
            (classParams.ClassroomStatus == ClassroomStatus.Ongoing && x.EndDate < DateOnly.FromDateTime(DateTime.Today) && x.EndDate > DateOnly.FromDateTime(DateTime.Today))))))
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
            if (applyPaging)
            {
                ApplyPaging(classParams.PageSize * (classParams.Page - 1),
                classParams.PageSize);
            }
        }
    }
}
