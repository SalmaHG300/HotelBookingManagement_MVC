namespace HotelManagement_MVC.ViewModel
{
    public class BookingExperienceEditVM
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public int NumAdults { get; set; }
        public string? SpecialRequest { get; set; }
    }
}
