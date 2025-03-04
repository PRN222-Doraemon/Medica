using Core.Entities.Identity;

namespace Core.Interfaces.Services
{
    public interface ILecturerService
    {
        public Task<IEnumerable<Lecturer>> GetLecturersAsync();
    }
}
