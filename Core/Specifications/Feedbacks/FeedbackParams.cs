using Core.Entities;

namespace Core.Specifications.Feedbacks
{
    public class FeedbackParams : PagingParams
    {
        private float _minRating;
        public float? MinRating
        {
            get { return _minRating; }
            set { _minRating = value!.Value < 0 ? 0 : value.Value; }
        }

        private float _maxRating;
        public float? MaxRating
        {
            get { return _maxRating; }
            set { _maxRating = value!.Value < 0 ? 0 : value.Value; }
        }

        public string? searchContent { get; set; }

        public FeedbackStatus? Status { get; set; }

        public int? StudentId { get; set; }

        public int? CourseId { get; set; }
    }
}
