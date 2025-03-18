using Core.Entities;

namespace MedicaWeb_MVC.ViewModels.Shared
{
    public class ListVM<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public PagingVM? PagingInfo { get; set; }
        public SearchbarVM? SearchValue { get; set; }
        public ClassroomStatus? ClassroomStatus { get; set; }
    }
}
