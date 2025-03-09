using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications;
using Core.Specifications.Courses;
using Core.Specifications.Feedbacks;

namespace Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IUnitOfWork _unitOfWork;

        // ==============================
        // === Constructors
        // ==============================

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================k

        public async Task<IEnumerable<Feedback>> GetAllFeedbacks(ISpecification<Feedback> spec)
        {
            return await _unitOfWork.Repository<Feedback>().ListAsync(spec);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            var spec = new FeedbackSpecification(id);
            return await _unitOfWork.Repository<Feedback>().GetEntityWithSpec(spec);
        }

        public async Task UpdateFeedBack(Feedback feedback)
        {
            _unitOfWork.Repository<Feedback>().Update(feedback);
            await _unitOfWork.Repository<Feedback>().SaveAllAsync();
        }
        public async Task CreateFeedBack(Feedback feedback)
        {
            feedback.CreatedAt = DateTime.Now;
            feedback.UpdatedAt = DateTime.Now;
            _unitOfWork.Repository<Feedback>().Add(feedback);
            await _unitOfWork.Repository<Feedback>().SaveAllAsync();
        }

        public async Task DeleteFeedback(Feedback feedback)
        {
            feedback.Status = FeedbackStatus.Disabled;
            _unitOfWork.Repository<Feedback>().Update(feedback);
            await _unitOfWork.Repository<Feedback>().SaveAllAsync();
        }
    }
}
