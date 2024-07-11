using HotelManagement_MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement_MVC.ViewModel
{
    public class BookingDiningEditVM
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int NumAdults { get; set; }
        //public int Price { get; set; }
        public string? SpecialRequest { get; set; }
        public int DiningId { get; set; }
        //public SelectList DiningOptions { get; set; }
    }
}
