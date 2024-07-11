using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.Models
{
    public class HotelRoom
    {
        public int Id { get; set; }

        [ForeignKey("HotelFloor")]
        public int HotelFloorId { get; set; }
        public HotelFloor HotelFloor { get; set; }
        [ForeignKey("HotelRoomType")]
        public int HotelRoomTypeId { get; set; }
        public HotelRoomType HotelRoomType { get; set; }      
        public List<BookingRoom>? BookingRoom { get; set; }
   
    }
}
