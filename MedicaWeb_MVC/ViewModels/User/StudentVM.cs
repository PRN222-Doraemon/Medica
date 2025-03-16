namespace MedicaWeb_MVC.ViewModels.User
{
    public class StudentVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? ImageUrl { get; set; } = default!;
        public float Grade { get; set; } = 8;
    }
}
