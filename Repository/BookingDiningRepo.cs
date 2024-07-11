using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_MVC.Repository
{
    public class BookingDiningRepo : IBookingDiningRepo
    {
        public HotelContext context;
        public BookingDiningRepo(HotelContext _context)
        {
            context = _context;
        }
        public List<BookingDining> GetAll()
        {
            return context.BookingDinings.Include(d=>d.Dining).ToList();
        }

        public BookingDining GetById(int Id)
        {
            return context.BookingDinings.FirstOrDefault(d => d.Id == Id);
        }
        public void Insert(BookingDining obj)
        {
            context.Add(obj);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public int? DuplicatePrice(BookingDining bookingDining)
        {
            int? NewPrice = 0;
            NewPrice = bookingDining.Price * bookingDining.NumAdults;
            return NewPrice;
        }
        public void Update(BookingDining obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            BookingDining bookingDining = GetById(Id);
            context.Remove(bookingDining);
        }
        public Dining GetDiningById(int Id)
        {
            return context.Dinings.FirstOrDefault(d => d.Id == Id);
        }
        public void UpdateDining(Dining obj)
        {
            context.Update(obj);
        }

      
    }
}
