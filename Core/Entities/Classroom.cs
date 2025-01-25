using Core.Entities.Constants;
using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Classroom : BaseEntity
    {
        public ClassroomMode Mode { get; set; }
        public ClassroomStatus Status { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public string Description { get; set; }
        public int MaxParticipant { get; set; }
        public decimal Price { get; set; }

        // 1 course - M classroom 
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // 1 lecturer - M classroom
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }
    }
}
