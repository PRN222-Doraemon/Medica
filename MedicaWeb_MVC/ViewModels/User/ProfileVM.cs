using System.ComponentModel.DataAnnotations;

namespace MedicaWeb_MVC.ViewModels.User
{
    public class ProfileVM
    {
        public string? ProfileImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        public string? UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}