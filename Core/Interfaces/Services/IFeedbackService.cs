using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces.Services
{
    public interface IFeedbackService
    {
        public Task<IEnumerable<Feedback>> GetAllFeedbacks(ISpecification<Feedback> spec);
        public Task<Feedback> GetFeedbackByIdAsync(int id);

        //public Task<IEnumerable<Course>> TopCourseRatingAsync(int count);

        public Task CreateFeedBack(Feedback feedback);
        public Task UpdateFeedBack(Feedback feedback);
        public Task DeleteFeedback (Feedback feedback);
    }
}
