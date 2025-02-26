using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Courses
{
    public class CourseSpecification : BaseSpecification<Course>
    {
        public CourseSpecification(CourseParams courseParam, bool applyPaging = true)
            : base(c => (string.IsNullOrEmpty(courseParam.Search) || c.Name.ToLower().Contains(courseParam.Search)) &&
            (!courseParam.CategoryID.HasValue || courseParam.CategoryID == c.CategoryID) &&
            (!courseParam.Status.HasValue || courseParam.Status == c.Status) &&
            (!courseParam.CreatedByUserId.HasValue || courseParam.CreatedByUserId == c.CreatedByUserID))
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.CreatedBy);
            AddCustomInclude(x => x.Include(c => c.CourseChapters)
                                    .ThenInclude(cc => cc.Resources));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.SrcComment));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.User));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.ReplyComments)
                                    .ThenInclude(rc => rc.User));
            AddCustomInclude(x => x.Include(c => c.Feedbacks)
                                    .ThenInclude(f => f.Student));
            AddCustomInclude(x => x.Include(c => c.Classrooms)
                                    .ThenInclude(f => f.OrderDetails));
            if (applyPaging)
            {
                ApplyPaging(courseParam.PageSize * (courseParam.Page - 1),
                courseParam.PageSize);
            }
        }

        public CourseSpecification(int id)
            : base(c => c.Id == id)
        {
            AddInclude(x => x.CreatedBy);
            AddInclude(x => x.Category);
            AddCustomInclude(x => x.Include(c => c.CourseChapters)
                                    .ThenInclude(cc => cc.Resources));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.SrcComment));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.User));
            AddCustomInclude(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.ReplyComments)
                                    .ThenInclude(rc => rc.User));
            AddCustomInclude(x => x.Include(c => c.Feedbacks)
                                    .ThenInclude(f => f.Student));
            AddCustomInclude(x => x.Include(c => c.Classrooms)
                                    .ThenInclude(f => f.OrderDetails));
        }
    }
}
