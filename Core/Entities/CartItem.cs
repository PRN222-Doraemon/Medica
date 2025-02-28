using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CartItem
    {
        public int ClassRoomId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
