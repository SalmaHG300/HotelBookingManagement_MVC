using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IBookingDiningRepo
    {
        public List<BookingDining> GetAll();
        public BookingDining GetById(int Id);
        public void Insert(BookingDining obj);
        public void Save();
        //public List<BookingDining> Search(string search);
        public void Delete(int Id);
        public Dining GetDiningById(int Id);
        public void UpdateDining(Dining obj);
        public void Update(BookingDining obj);
        public int? DuplicatePrice(BookingDining bookingDining);
    }
}
