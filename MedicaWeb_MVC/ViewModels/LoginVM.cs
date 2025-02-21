using System.ComponentModel.DataAnnotations;

namespace MedicaWeb_MVC.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "User Name")]
        public required string Email { get; set; }


        [Display(Name = "Password")]
        public required string Password { get; set; }
    }
}
