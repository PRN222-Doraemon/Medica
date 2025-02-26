//using Core.Entities;
//using Core.Specifications;

//namespace Infrastructure.Specifications.ClassSpecifications
//{
//    public class ClassroomWithCourseSpecification : BaseSpecification<Classroom>
//    {
//        public ClassroomWithCourseSpecification(LecturerClassroom lecturerClassroomVM) :
//            base(x =>
//                (string.IsNullOrEmpty(lecturerClassroomVM.Keyword) || x.Course.Name.Contains(lecturerClassroomVM.Keyword)) &&
//                (lecturerClassroomVM.CategoryId == 0 || (lecturerClassroomVM.CategoryId != 0 && x.Course.Category.Id == lecturerClassroomVM.CategoryId)) &&
//                (lecturerClassroomVM.ClassroomStatus == "All" || (lecturerClassroomVM.ClassroomStatus != "All" && x.Status == Enum.Parse<ClassroomStatus>(lecturerClassroomVM.ClassroomStatus!)))
//            // &&
//            // (lecturerClassroomVM.IsMyClass == false || (lecturerClassroomVM.IsMyClass == true && x.Lecturer.Id == lecturerClassroomVM.LecturerId))
//            )
//        {
//            AddInclude(c => c.Course);
//            AddInclude(c => c.Lecturer);
//            switch (lecturerClassroomVM.SortOrder)
//            {
//                case "newest":
//                    AddOrderBy(c => c.UpdatedAt);
//                    break;
//                case "oldest":
//                    AddOrderByDescending(c => c.UpdatedAt);
//                    break;
//                case "name":
//                    AddOrderBy(c => c.Course.Name);
//                    break;
//                case "nameDesc":
//                    AddOrderByDescending(c => c.Course.Name);
//                    break;
//            }
//        }
//    }
//}