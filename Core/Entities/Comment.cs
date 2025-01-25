using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Comment : BaseEntity
    {
        public int UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public int? SrcCommentID { get; set; }
        public Comment? SrcComment { get; set; }
        public IEnumerable<Comment> ReplyComments { get; set; } = new List<Comment>();
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = CommentStatus.Posted.ToString();
    }

    public enum CommentStatus
    {
        [EnumMember(Value = "Posted")]
        Posted,
        [EnumMember(Value = "Edited")]
        Edited,
        [EnumMember(Value = "Deleted")]
        Deleted,
    }
}
