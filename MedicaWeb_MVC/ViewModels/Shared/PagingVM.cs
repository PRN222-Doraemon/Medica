using Core.Constants;

namespace MedicaWeb_MVC.ViewModels.Shared
{
    public class PagingVM
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; } = AppCts.Display.MAX_VISIBLE_COURSES;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
    }
}
