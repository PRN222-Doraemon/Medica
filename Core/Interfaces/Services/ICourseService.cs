using Core.Entities;
using Core.Specifications;

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
