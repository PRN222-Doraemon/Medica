using System.ComponentModel.DataAnnotations;

namespace MedicaWeb_MVC.ViewModels.User
{
    public class LoginVM
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;


        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;


        [Display(Name = "Remember Me")]
        public bool IsRememberMe { get; set; } = false;

        public string? ReturnUrl { get; set; }
    }
}
