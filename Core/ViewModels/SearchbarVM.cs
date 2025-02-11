namespace Core.ViewModels
{
    public class SearchbarVM
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string SearchText { get; set; } = string.Empty;
    }
}