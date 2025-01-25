namespace Core.Entities.Identity
{
    public class Student : ApplicationUser
    {
        // 1 Student - 1 User
        public virtual ApplicationUser User { get; set; }
    }
}
