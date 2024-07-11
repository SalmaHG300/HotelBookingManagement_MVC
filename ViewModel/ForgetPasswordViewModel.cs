using System.ComponentModel.DataAnnotations;

namespace HotelManagement_MVC.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is requires")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
