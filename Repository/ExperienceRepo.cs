using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_MVC.Repository
{
    public class ExperienceRepo : IExperienceRepo
    {
        public HotelContext context;
        public ExperienceRepo(HotelContext _context)
        {
            context = _context;
        }
        public List<Experience> GetAll()
        {
            return context.Experiences.Include(i =>i.Type).ToList();
        }

        public Experience GetById(int Id)
        {
            return context.Experiences.Include(i => i.Type).FirstOrDefault(r => r.Id == Id);
        }


        public void Insert(Experience obj)
        {
            context.Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Experience> Search(string search)
        {
            return context.Experiences
                    .Where(i => i.Name.Contains(search)).Include(i => i.Type)
                    .ToList();
        }

        public void Update(Experience obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            Experience hotelRoomType = GetById(Id);
            context.Remove(hotelRoomType);
        }

    }
}
