using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_MVC.Repository
{
    public class BookingExperienceRepo : IBookingExperienceRepo
    {
        public HotelContext context;
        public BookingExperienceRepo(HotelContext _context)
        {
            context = _context;
        }
        public List<BookingExperience> GetAll()
        {
            return context.BookingExperiences.Include(e => e.Experience).ToList();
        }

        public BookingExperience GetById(int Id)
        {
            return context.BookingExperiences.FirstOrDefault(d => d.Id == Id);
        }


        public void Insert(BookingExperience obj)
        {
            context.Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public int? DuplicatePrice(BookingExperience bookingExperience)
        {
            int? NewPrice = 0;
            NewPrice = bookingExperience.Price * bookingExperience.NumAdults;
            return NewPrice;
        }
        //public List<BookingDining> Search(string search)
        //{
        //    return context.BookingDinings
        //            .Where(d => d.ApplicationUserId == string.Parse(search))
        //            .ToList();
        //}

        public void Update(BookingExperience obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            BookingExperience bookingExperience = GetById(Id);
            context.Remove(bookingExperience);
        }
        public Experience GetExperienceById(int Id)
        {
            return context.Experiences.FirstOrDefault(d => d.Id == Id);
        }
        public void UpdateExperience(Experience obj)
        {
            context.Update(obj);
        }

    }
}
