using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNewsAsync(News news)
        {
            _unitOfWork.Repository<News>().Add(news);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<News>> GetNewsAsync(ISpecification<News> spec)
        {
            return await _unitOfWork.Repository<News>().ListAsync(spec);
        }

        public async Task<News> GetNewsWithSpec(ISpecification<News> spec)
        {
            return await _unitOfWork.Repository<News>().GetEntityWithSpec(spec);
        }

        public async Task UpdateNewsAsync(News news)
        {
            _unitOfWork.Repository<News>().Update(news);
            await _unitOfWork.CompleteAsync();
        }
    }
}