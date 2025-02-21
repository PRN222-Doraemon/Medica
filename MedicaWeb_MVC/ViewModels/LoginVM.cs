using System.ComponentModel.DataAnnotations;

namespace MedicaWeb_MVC.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "User Name")]
        public required string UserName { get; set; }


        [Display(Name = "Password")]
        public required string Password { get; set; }


        [Display(Name = "Remember Me")]
        public bool IsRememberMe { get; set; } = false;
    }
}
