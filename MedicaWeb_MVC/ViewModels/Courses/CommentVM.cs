using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Courses
{
    public class CommentVM
    {
        public int Id { get; set; }
        public int ClassID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public IEnumerable<CommentVM> ReplyComments { get; set; } = new List<CommentVM>();
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public CommentStatus Status { get; set; }
    }
}
