using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Course : BaseEntity
    {


        // 1 Course - M Classroom
        public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();
    }
}
