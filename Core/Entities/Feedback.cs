using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Feedback : BaseEntity
    {
        public decimal Rating { get; set; }
        public string FeedbackContent { get; set; }
        public FeedbackStatus Status { get; set; }


        // 1 Student - M Feedback
        public int StudentId { get; set; }
        public Student Student { get; set; }


        // 1 Lecturer - M Feedback
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }


        // 1 Classroom - M Feedback
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }

    public enum FeedbackStatus
    {
        Enabled,
        Disabled
    }
}
