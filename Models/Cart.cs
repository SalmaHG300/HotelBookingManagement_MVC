using HotelManagement_MVC.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.Models
{
    public class Cart
    {
        public int Id { get; set; }
            public List<BookingDining>? BookingDinings { get; set; }
            public List<BookingRoom>? BookingRooms { get; set; }
            public List<BookingExperience>? BookingExperiences { get; set; }
            public PaymentMethod paymentMethod { get; set; }
            public int ShippingPrice { get; set; }
            public PaymentStatus paymentStatus { get; set; }
            [ForeignKey("ApplicationUser")]
            public string ApplicationUserId { get; set; }
            public ApplicationUser ApplicationUser { get; set; }
    }
}
