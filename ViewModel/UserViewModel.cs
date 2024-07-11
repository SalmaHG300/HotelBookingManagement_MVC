using System.ComponentModel.DataAnnotations;

namespace HotelManagement_MVC.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel() 
        { 
           Id=Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is requires")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [RegularExpression("^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }
        //public IEnumerable<string> Roles {  get; set; }  


    }
}
