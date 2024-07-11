using HotelManagement_MVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.Models
{
    public class BookingRoom
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("HotelRoom")]
        public int HotelRoomId { get; set; }
        public HotelRoom HotelRoom { get; set; }

        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer? Offer { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; } = 0;
        public int? TotalPrice { get; set; }
        public int? TotalDays { get; set; }
        [Range(1, 10)]
        public int? NumOfRooms { get; set; }= 1;
        public string? SpecialRequest { get; set; }
    }
}
