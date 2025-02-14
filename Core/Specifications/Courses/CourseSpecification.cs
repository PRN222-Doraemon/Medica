using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.Courses
{
    public class CourseSpecification : BaseSpecification<Course>
    {
        public CourseSpecification(CourseParams courseParam)
            :base(c => (string.IsNullOrEmpty(courseParam.Search) ||  courseParam.Search == c.Name) &&
            (!courseParam.CategoryID.HasValue || courseParam.CategoryID == c.CategoryID) &&
            (!courseParam.Status.HasValue)|| courseParam.Status == c.Status)
        {
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
        }

        public CourseSpecification(int id)
            :base(c => c.Id ==  id)
        {
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
        }
    }
}
