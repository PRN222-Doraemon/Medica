using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Specifications.ClassSpecifications
{
    public class ClassroomSpecifications : BaseSpecification<Classroom>
    {
        public ClassroomSpecifications(ClassroomSpecParams classroomSpecParams)
            : base(x =>
                (string.IsNullOrEmpty(classroomSpecParams.ClassroomStatus) || classroomSpecParams.ClassroomStatus == x.Status.ToString()) &&
                (classroomSpecParams.CourseId == 0 || classroomSpecParams.CourseId == x.CourseId))
        {

        }
    }
}