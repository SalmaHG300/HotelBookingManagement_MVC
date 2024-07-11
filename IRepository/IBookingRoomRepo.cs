using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IBookingRoomRepo
    {
        public List<BookingRoom> GetAll();
        public BookingRoom GetById(int Id);
        public void Insert(BookingRoom obj);
        public void Save();
        //public List<BookingRoom> Search(string search);
        public void Delete(int Id);
        public HotelRoomType GetRoomTypeById(int Id);
        public void Update(BookingRoom obj);
        public int? DuplicatePrice(BookingRoom bookingRoom);
    }
}
