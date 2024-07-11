using System.ComponentModel.DataAnnotations;

namespace HotelManagement_MVC.ViewModel
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is requires")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
