using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications;
using Core.Specifications.Courses;

namespace Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCourseAsync(Course course)
        {
            _unitOfWork.Repository<Course>().Add(course);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await GetCourseByIdAsync(id);
            course.Status = CourseStatus.Inactive;
            _unitOfWork.Repository<Course>().Update(course);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var spec = new CourseSpecification(id);
            var course = await _unitOfWork.Repository<Course>().GetEntityWithSpec(spec);
            return course;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(ISpecification<Course> spec)
        {
            return await _unitOfWork.Repository<Course>().ListAsync(spec);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            _unitOfWork.Repository<Course>().Update(course);
            await _unitOfWork.CompleteAsync();
        }
    }
}
