using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.ViewModel
{
    public class BookingDiningVM
    {
        public int DiningId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string Duration { get; set; }
        public int Price { get; set; }
        public string TimeOfDay { get; set; }
        public DateTime Date { get; set; }
        public int NumAdults { get; set; }
        public string? SpecialRequest { get; set; }
        public string applicationUserId { get; set;}
    }
}
