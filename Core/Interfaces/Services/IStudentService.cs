using Core.Entities.Identity;

namespace Core.Interfaces.Services
{
    public interface IStudentService
    {
        public Task<IEnumerable<Student>> GetAllStudentsAsync();
    }
}