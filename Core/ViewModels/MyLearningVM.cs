using System.Runtime.Serialization;

namespace MedicaWeb_MVC.ViewModels
{
    public class MyLearningVM
    {
        public IEnumerable<ClassVM> Classes { get; set; } = new List<ClassVM>();
        public SelectedFilter SelectedFilter { get; set; } = SelectedFilter.All;

    }

    public enum SelectedFilter
    {
        [EnumMember(Value = "All")]
        All,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "InProgress")]
        InProgress,
        [EnumMember(Value = "UpComing")]
        UpComing
    }
}
