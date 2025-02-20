using Core.Entities.Identity;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class CourseCreateVM
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public int CategoryID { get; set; }
        public int CreatedByUserID { get; set; } = 1;
        public TimeSpan Duration { get; set; }
        public IEnumerable<CourseChapterVM> CourseChapters { get; set; } = new List<CourseChapterVM>();
    }
}

