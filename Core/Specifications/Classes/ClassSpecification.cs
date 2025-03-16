using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Classes
{
    public class ClassSpecification : BaseSpecification<Classroom>
    {
        public ClassSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Course);
            AddInclude(x => x.Lecturer);
            AddCustomInclude(c => c.Include(c => c.OrderDetails).ThenInclude(od => od.Order).ThenInclude(o => o.Student));
            AddCustomInclude(c => c.Include(c => c.Course).ThenInclude(c => c.CourseChapters).ThenInclude(c => c.Resources));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.SrcComment));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.User));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.ReplyComments)
                                    .ThenInclude(rc => rc.User));
        }

        public ClassSpecification(ClassParams classParams, bool applyPaging = true) :
            base(x => (string.IsNullOrEmpty(classParams.Search) || x.Course.Name.ToLower().Contains(classParams.Search)) &&
            (!classParams.CategoryId.HasValue || classParams.CategoryId == x.Course.Category.Id) &&
            (!classParams.CourseId.HasValue || classParams.CourseId == x.Course.Id) &&
            (!classParams.LecturerId.HasValue || classParams.LecturerId == x.LecturerId) &&
            (x.Status != ClassroomStatus.Cancelled) && ((!classParams.ClassroomStatus.HasValue) ||
            (classParams.ClassroomStatus == ClassroomStatus.Upcoming && x.StartDate > DateOnly.FromDateTime(DateTime.Today)) ||
            (classParams.ClassroomStatus == ClassroomStatus.Completed && x.EndDate < DateOnly.FromDateTime(DateTime.Today)) ||
            (classParams.ClassroomStatus == ClassroomStatus.Ongoing && x.StartDate < DateOnly.FromDateTime(DateTime.Today) && x.EndDate > DateOnly.FromDateTime(DateTime.Today))))
        {
            AddCustomInclude(c => c.Include(c => c.Course).ThenInclude(c => c.Category));
            AddCustomInclude(c => c.Include(c => c.Course).ThenInclude(c => c.CourseChapters).ThenInclude(c => c.Resources));
            AddInclude(c => c.Lecturer);
            AddCustomInclude(c => c.Include(c => c.OrderDetails).ThenInclude(od => od.Order).ThenInclude(o => o.Student));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.SrcComment));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.User));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.ReplyComments)
                                    .ThenInclude(rc => rc.User));
            AddOrderByDescending(c => c.StartDate);

            if (applyPaging)
            {
                ApplyPaging(classParams.PageSize * (classParams.Page - 1),
                classParams.PageSize);
            }
        }
    }
}
