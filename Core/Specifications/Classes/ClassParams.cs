using Core.Entities;

namespace Core.Specifications.Classes
{
    public class ClassParams : PagingParams
    {
        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value?.ToLower() ?? string.Empty;
        }
        public int? CategoryId { get; set; }
        public int? CourseId { get; set; }
        public ClassroomStatus? ClassroomStatus { get; set; }
        public string? SortOrder { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? LecturerId { get; set; }
    }
}
