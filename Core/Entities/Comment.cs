﻿using Core.Entities.Identity;
using System.Runtime.Serialization;

namespace Core.Entities
{
    public class Comment : BaseEntity
    {
        public int UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int ClassID { get; set; }
        public Classroom Classroom { get; set; }
        public int? SrcCommentID { get; set; }
        public Comment? SrcComment { get; set; }
        public IEnumerable<Comment> ReplyComments { get; set; } = new List<Comment>();
        public string Title { get; set; }
        public string Details { get; set; }
        public CommentStatus Status { get; set; }
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
