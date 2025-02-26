using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task CreateClassAsync(Classroom classroom)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClassAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Course> GetClassByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Classroom>> GetClassesAsync(ISpecification<Classroom> spec)
        {
            return await _unitOfWork.Repository<Classroom>().ListAsync(spec);
        }

        public Task UpdateClassAsync(Classroom classroom)
        {
            throw new NotImplementedException();
        }
    }
}
