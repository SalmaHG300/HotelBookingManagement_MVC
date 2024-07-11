using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement_MVC.Models
{
    public class Dining
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }
        public string Duration { get; set; }
        public int Price { get; set; }
        public string TimeOfDay { get; set; }
        public List<BookingDining>? BookingDinings { get; set; }
    }
}