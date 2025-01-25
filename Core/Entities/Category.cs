namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
    }
}
