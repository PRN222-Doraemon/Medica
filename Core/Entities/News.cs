using System.Runtime.Serialization;

namespace Core.Entities
{
    public class News : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public NewsType NewsType { get; set; }
        public NewsStatus Status { get; set; }
    }

    public enum NewsStatus
    {
        [EnumMember(Value = "Active")]
        Active,

        [EnumMember(Value = "Disabled")]
        Disabled

    }
    public enum NewsType
    {
        [EnumMember(Value = "Blog")]
        Blog,

        [EnumMember(Value = "Program")]
        Program
    }

}
