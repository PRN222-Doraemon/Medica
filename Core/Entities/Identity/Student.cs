using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class Student : ApplicationUser
    {
        // 1 Student - 1 User
        public virtual ApplicationUser User { get; set; }
    }
}
