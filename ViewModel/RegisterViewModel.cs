using System.ComponentModel.DataAnnotations;

namespace HotelManagement_MVC.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "First Name")]
        public string Fname { get; set; }
        [Display(Name = "Last Name")]
        public string Lname { get; set; }
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is requires")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [RegularExpression("^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

		[Required(ErrorMessage ="Password is requires")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Conform password is requires")]
        [Compare("Password",ErrorMessage ="Password does not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } 
    }
}
