using Core.Entities;

namespace Core.Specifications.Classes
{
    public class ClassParams
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public ClassroomStatus? ClassroomStatus { get; set; }
        public string? SortOrder { get; set; }
    }
}
