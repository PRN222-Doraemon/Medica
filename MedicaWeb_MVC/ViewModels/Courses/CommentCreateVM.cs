
namespace MedicaWeb_MVC.ViewModels.Courses
{
    public class CommentCreateVM
    {
        public int UserID { get; set; }
        public int ClassID { get; set; }
        public int? SrcCommentID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}