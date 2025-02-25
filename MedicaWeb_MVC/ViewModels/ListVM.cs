using Core.ViewModels;

namespace MedicaWeb_MVC.ViewModels
{
    public class ListVM<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public PagingVM? PagingInfo { get; set; }
        public SearchbarVM? SearchValue { get; set; }
    }
}
