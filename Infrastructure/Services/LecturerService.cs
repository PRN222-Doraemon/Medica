using Core.Entities.Identity;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;

namespace Infrastructure.Services
{
    public class LecturerService : ILecturerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LecturerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Lecturer>> GetLecturersAsync()
        {
            return await _unitOfWork.Repository<Lecturer>().ListAllAsync();
        }
    }
}
