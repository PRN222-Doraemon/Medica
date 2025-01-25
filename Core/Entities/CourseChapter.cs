

namespace Core.Entities
{
    public class CourseChapter : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public IEnumerable<Resource> Resources { get; set; } = new List<Resource>();    

    }
}
