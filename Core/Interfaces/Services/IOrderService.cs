using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Classroom>> GetMyLearningByStudentId(int studentId, ClassroomStatus? classStatus);
    }
}
