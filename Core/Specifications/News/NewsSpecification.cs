using Core.Entities;

namespace Core.Specifications.News
{
    public class NewsSpecification : BaseSpecification<Entities.News>
    {
        public NewsSpecification(NewsParams newsParams, bool applyPaging = true)
            : base(n => (string.IsNullOrEmpty(newsParams.Search) || n.Title.Contains(newsParams.Search)) &&
                      (!newsParams.NewsType.HasValue || n.NewsType == newsParams.NewsType) &&
                      (!newsParams.Status.HasValue || n.Status == newsParams.Status))
        {
            if (applyPaging)
            {
                ApplyPaging(newsParams.PageSize * (newsParams.Page - 1), newsParams.PageSize);
            }
            AddOrderByDescending(n => n.CreatedAt);

        }

        public NewsSpecification(int id) : base(n => n.Id == id)
        {
        }
    }
}