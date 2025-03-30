using Core.Entities;

namespace Core.Specifications.News
{
    public class NewsParams : PagingParams
    {
        public string? Search { get; set; }
        public NewsType? NewsType { get; set; }
        public NewsStatus? Status { get; set; }
    }
}