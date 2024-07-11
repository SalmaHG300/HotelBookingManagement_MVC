using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.ViewModel
{
    public class AllBookingEventsViewModel
    {
        public int RoomId { get; set; }
        public int ExperienceId { get; set; }
        public int DiningId { get; set; }
        public List<BookingRoom> HotelRoomType { get; set; }
        public List<BookingDining> Dining { get; set; }
        public List<BookingExperience> Experience { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
