using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class Lecturer : ApplicationUser
    {
        // 1 Lecturer - 1 User
        public virtual ApplicationUser User { get; set; }

        // 1 Lecturer - M Classroom
        public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();
    }
}
