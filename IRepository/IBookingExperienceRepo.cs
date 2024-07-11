using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IBookingExperienceRepo
    {
        public List<BookingExperience> GetAll();
        public BookingExperience GetById(int Id);
        public void Insert(BookingExperience obj);
        public void Save();
        public int? DuplicatePrice(BookingExperience bookingExperience);
        public void Update(BookingExperience obj);
        public void Delete(int Id);
        public Experience GetExperienceById(int Id);
        public void UpdateExperience(Experience obj);
    }
}
