using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications;
using Core.Specifications.Classes;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.WebSockets;

namespace Infrastructure.Services
{
    public class ClassService : IClassService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IUnitOfWork _unitOfWork;

        // ==============================
        // === Constructors
        // ==============================

        public ClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================

        public async Task CreateClassAsync(Classroom classroom)
        {
            _unitOfWork.Repository<Classroom>().Add(classroom);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteClassAsync(int id)
        {
            var classroom = await GetClassByIdAsync(id);
            if (classroom == null)
                throw new KeyNotFoundException("This classroom does not exist");
            if (classroom.OrderDetails != null && classroom.OrderDetails.Count > 0)
                throw new InvalidOperationException("Deletion failed: This class cannot be deleted because there are enrolled students");
            classroom.Status = ClassroomStatus.Cancelled;
            _unitOfWork.Repository<Classroom>().Update(classroom);
            await _unitOfWork.Repository<Classroom>().SaveAllAsync();
        }

        public async Task<Classroom?> GetClassAsync(ISpecification<Classroom> spec)
        {
            return await _unitOfWork.Repository<Classroom>().GetEntityWithSpec(spec);
        }

        public async Task<Classroom> GetClassByIdAsync(int id)
        {
            var spec = new ClassSpecification(id);
            var classroom = await _unitOfWork.Repository<Classroom>().GetEntityWithSpec(spec);
            classroom.Comments = classroom.Comments.Where(c => c.SrcComment == null).ToList();
            return classroom;
        }

        public async Task<IEnumerable<Classroom>> GetClassesAsync(ISpecification<Classroom> spec)
        {
            var classes = await _unitOfWork.Repository<Classroom>().ListAsync(spec);
            return classes;
        }

        public async Task UpdateClassAsync(Classroom classroom)
        {
            _unitOfWork.Repository<Classroom>().Update(classroom);
            await _unitOfWork.Repository<Classroom>().SaveAllAsync();
        }
    }
}
