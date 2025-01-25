using Microsoft.AspNetCore.Identity;
using MindSpace.Domain.Commons.Constants;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int> //Using int as key type
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(-1)]
        public string? ImageUrl { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public UserStatus Status { get; set; } = UserStatus.Enabled;
    }
}