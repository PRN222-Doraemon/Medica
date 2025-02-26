using Core.Interfaces.Repos;
using Core.Interfaces.Services;

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


    }
}
