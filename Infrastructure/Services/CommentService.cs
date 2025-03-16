using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;

namespace Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddComment(Comment comment)
        {
            _unitOfWork.Repository<Comment>().Add(comment);
            await _unitOfWork.CompleteAsync();
        }
    }
}
