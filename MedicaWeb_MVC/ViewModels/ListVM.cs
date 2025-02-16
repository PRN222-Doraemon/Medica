namespace MedicaWeb_MVC.ViewModels
{
    public class ListVM<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PagingVM PagingInfo { get; set; }
    }
}
