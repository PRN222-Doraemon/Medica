namespace Core.Entities.Identity
{
    public class Student : ApplicationUser
    {
        // 1 Student - 1 User
        public virtual ApplicationUser User { get; set; }

        // 1 Student - M Feedbacks
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
