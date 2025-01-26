using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int> //Using int as key type
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ImageUrl { get; set; } = default!;

        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public UserStatus Status { get; set; } = UserStatus.Enabled;

        // 1 Student - 1 User
        public virtual Student Student { get; set; }

        // 1 Student - 1 User
        public virtual Employee Employee { get; set; }

        // 1 Lecturer - 1 User
        public virtual Lecturer Lecturer { get; set; }
    }

    public enum UserStatus
    {
        [EnumMember(Value = "Enabled")]
        Enabled,

        [EnumMember(Value = "Disabled")]
        Disabled
    }
}