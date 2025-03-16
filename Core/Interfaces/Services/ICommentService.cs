using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task AddComment(Comment comment);
    }
}
