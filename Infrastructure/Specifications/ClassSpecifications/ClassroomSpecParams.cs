using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Specifications.ClassSpecifications
{
    public class ClassroomSpecParams
    {
        public int CourseId { get; set; }
        public string ClassroomStatus { get; set; }

    }
}