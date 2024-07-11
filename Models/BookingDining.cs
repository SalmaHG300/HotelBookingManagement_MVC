using HotelManagement_MVC.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.Models
{
    public class BookingDining
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime Date { get; set; }
        public  int NumAdults { get; set; }
        public int Price { get; set; }
        public string? SpecialRequest { get; set; }
        [ForeignKey("Dining")]
        public int DiningId { get; set; }
        public Dining Dining { get; set; }
    }
}
