using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces.Services
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetNewsAsync(ISpecification<News> spec);
        Task<News> GetNewsWithSpec(ISpecification<News> spec);
        Task UpdateNewsAsync(News news);
        Task CreateNewsAsync(News news);
    }
}