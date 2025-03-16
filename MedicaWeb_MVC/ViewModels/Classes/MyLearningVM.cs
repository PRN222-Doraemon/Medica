using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Classes
{
    public class MyLearningVM
    {
        public IEnumerable<ClassVM> Classes { get; set; } = new List<ClassVM>();
        public ClassroomStatus? SelectedStatus { get; set; }

    }
}
