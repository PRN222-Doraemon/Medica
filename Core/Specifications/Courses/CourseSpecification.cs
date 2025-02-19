using Core.Constants;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Courses
{
    public class CourseSpecification : BaseSpecification<Course>
    {
        public CourseSpecification(CourseParams courseParam)
            :base(c => (string.IsNullOrEmpty(courseParam.Search) || c.Name.ToLower().Contains(courseParam.Search)) &&
            (!courseParam.CategoryID.HasValue || courseParam.CategoryID == c.CategoryID) &&
            (!courseParam.Status.HasValue|| courseParam.Status == c.Status) &&
            (!courseParam.CreatedByUserId.HasValue || courseParam.CreatedByUserId == c.CreatedByUserID))
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.CreatedBy);
            CustomIncludes.Add(x => x.Include(c => c.CourseChapters)
                                    .ThenInclude(cc => cc.Resources)
                                    .ThenInclude(r => r.CreatedBy));
            CustomIncludes.Add(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.SrcComment));
            CustomIncludes.Add(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.User));
            CustomIncludes.Add(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.ReplyComments)
                                    .ThenInclude(rc => rc.User));
            CustomIncludes.Add(x => x.Include(c => c.Feedbacks)
                                    .ThenInclude(f => f.Student));
            ApplyPaging(courseParam.PageSize * (courseParam.PageIndex - 1),
                courseParam.PageSize);
            
        }

        public CourseSpecification(int id)
            :base(c => c.Id ==  id)
        {
            AddInclude(x => x.CreatedBy);
            AddInclude(x => x.Category);
            CustomIncludes.Add(x => x.Include(c => c.CourseChapters)
                                    .ThenInclude(cc => cc.Resources)
                                    .ThenInclude(r => r.CreatedBy));
            CustomIncludes.Add(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.SrcComment));
            CustomIncludes.Add(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.User));
            CustomIncludes.Add(x => x.Include(c => c.Comments)
                                    .ThenInclude(cc => cc.ReplyComments)
                                    .ThenInclude(rc => rc.User));
            CustomIncludes.Add(x => x.Include(c => c.Feedbacks)
                                    .ThenInclude(f => f.Student));
        }

    }
}
