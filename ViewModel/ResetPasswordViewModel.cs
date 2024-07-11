using System.ComponentModel.DataAnnotations;

namespace HotelManagement_MVC.ViewModel
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is requires")]
		[DataType(DataType.Password)]
		public string newPassword { get; set; }
		[Required(ErrorMessage = "Conform password is requires")]
		[Compare("newPassword", ErrorMessage = "Password does not match")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }


		//public string Email { get; set; }
		//public string Token { get; set; }
	}
}
