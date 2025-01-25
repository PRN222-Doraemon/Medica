namespace Core.Entities.Identity
{
    public class Lecturer : ApplicationUser
    {
        // 1 Lecturer - 1 User
        public virtual ApplicationUser User { get; set; }

        // 1 Lecturer - M Classroom
        public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();
       
        // 1 Lecturer - M Feedbacks
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();

    }
}
