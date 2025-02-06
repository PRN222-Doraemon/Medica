namespace MedicaWeb_MVC.ViewModels
{
    public class SearchbarVM
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string SearchText { get; set; } = string.Empty;
    }
}
