using Core.Entities;

namespace Core.Specifications.Feedbacks
{
    public class FeedbackSpecification : BaseSpecification<Feedback>
    {
        public FeedbackSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Student);
            AddInclude(x => x.Course);
        }

        public FeedbackSpecification(FeedbackParams specParams, bool applyPaging) : base(x =>
            (string.IsNullOrEmpty(x.FeedbackContent) || x.FeedbackContent.ToLower().Contains(specParams.searchContent!)) &&
            (!specParams.StudentId.HasValue || x.StudentId == specParams.StudentId) &&
            (!specParams.MinRating.HasValue || (float)x.Rating >= specParams.MinRating.Value) &&
            (!specParams.MinRating.HasValue || (float)x.Rating <= specParams.MaxRating.Value) &&
            (!specParams.CourseId.HasValue || x.CourseId == specParams.CourseId) &&
            (!specParams.StudentId.HasValue || x.StudentId == specParams.StudentId))
        {
            AddInclude(x => x.Student);
            AddInclude(x => x.Course);
            if (applyPaging)
            {
                ApplyPaging(specParams.PageSize * (specParams.Page - 1),
                specParams.PageSize);
            }
            AddOrderBy(x => x.Id);
        }
    }
}
