namespace MedicaWeb_MVC.ViewModels
{
    public class CourseCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        //public int CreatedByUserID { get; set; }
        public TimeSpan Duration { get; set; }
        public IEnumerable<CourseChapterCreateVM> CourseChapters { get; set; } = new List<CourseChapterCreateVM>();
    }
}

