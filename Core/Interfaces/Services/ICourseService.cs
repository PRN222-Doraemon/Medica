using Core.Entities;
using Core.Specifications;
using Core.Specifications.Courses;

namespace Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCoursesAsync(ISpecification<Course> spec);
        Task<Course> GetCourseByIdAsync(int id);
        Task CreateCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);

    }
}
