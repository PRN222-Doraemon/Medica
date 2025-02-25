using Core.Entities;

namespace Core.ViewModels
{
    public class LecturerClassroomVM
    {
        public List<Classroom>? Classrooms { get; set; }
        public string? Keyword { get; set; }
        public int CategoryId { get; set; } = 0;
        public string? ClassroomStatus { get; set; } = "All";
        public string? SortOrder { get; set; } = "newest";
    }
}