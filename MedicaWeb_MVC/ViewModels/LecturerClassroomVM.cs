using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace MedicaWeb_MVC.ViewModels
{
    public class LecturerClassroomVM
    {
        public List<Classroom>? Classrooms { get; set; }
        public List<Category>? Categories { get; set; }
    }
}