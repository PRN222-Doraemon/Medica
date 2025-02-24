using Core.Constants;

namespace Core.Specifications
{
    public class PagingParams
    {
        private const int MaxPageSize = 30;
        public int Page { get; set; } = 1;

        private int _pageSize = AppCts.Display.MAX_VISIBLE_COURSES;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
