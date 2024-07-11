using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement_MVC.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Fname { get; set; }

        public string Lname { get; set; }
 
        public string Address { get; set; }
    }
}