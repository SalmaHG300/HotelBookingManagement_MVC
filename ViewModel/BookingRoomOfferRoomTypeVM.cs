using HotelManagement_MVC.Enums;
using HotelManagement_MVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.ViewModel
{
    public class BookingRoomOfferRoomTypeVM
    {
        //public int HotelFloorId { get; set; }
        //public List<HotelFloor> FloorList { get; set; }

        //public int HotelRoomTypeId { get; set; }
        //public List<HotelRoomType> RoomTypeList { get; set; }
        //public int OfferId { get; set; }
        //public DateTime CheckInDate { get; set; }
        //public DateTime CheckOutDate { get; set; }
        //public int NumAdults { get; set; }
        //public int NumChildren { get; set; } = 0;

        //[Range(1, 10)]
        //public int? NumOfRooms { get; set; } = 1;
        //public string? SpecialRequest { get; set; }
        //public PaymentStatus PaymentStatus { get; set; }
        //public string applicationUserId { get; set; }
           
        public int Id { get; set; }
        public int HotelFloorId { get; set; }
        public int HotelRoomTypeId { get; set; }
        public int OfferId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        [Range(1, 10)]
        public int NumAdults { get; set; }
        public int NumChildren { get; set; } = 0;
        [Range(1, 10)]
        public int? NumOfRooms { get; set; } = 1;
        public string? SpecialRequest { get; set; }
        

    }
}