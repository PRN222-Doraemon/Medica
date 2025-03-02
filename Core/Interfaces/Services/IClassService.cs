using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces.Services
{
    public interface IClassService
    {
        Task<IEnumerable<Classroom>> GetClassesAsync(ISpecification<Classroom> spec);
        Task<Classroom?> GetClassAsync(ISpecification<Classroom> spec);
        Task<Course> GetClassByIdAsync(int id);
        Task CreateClassAsync(Classroom classroom);
        Task UpdateClassAsync(Classroom classroom);
        Task DeleteClassAsync(int id);
    }
}
