using Core.Entities;

namespace Core.Specifications.Feedbacks
{
    public class FeedbackParams : PagingParams
    {
        public string? searchContent { get; set; }
        public FeedbackStatus? Status { get; set; }
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
    }
}
