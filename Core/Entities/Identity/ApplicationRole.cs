using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class ApplicationRole : IdentityRole<int> // Use int as the key type
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
