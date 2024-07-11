using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_MVC.Repository
{
    public class HotelRoomRepo : IHotelRoomRepo
    {
        public HotelContext context;
        public HotelRoomRepo(HotelContext _context)
        {
            context = _context;
        }

        public List<HotelRoom> GetAll()
        {
            return context.HotelRooms.Include(f => f.HotelFloor).Include(h =>h.HotelRoomType ).ToList();
        }

        public HotelRoom GetById(int Id)
        {
            return context.HotelRooms.FirstOrDefault(d => d.Id == Id);
        }


        public void Insert(HotelRoom obj)
        {
            context.Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<HotelRoom> Search(string search)
        {
            return context.HotelRooms
                    .Where(h => h.HotelRoomTypeId == int.Parse(search))
                    .ToList();
        }

        public void Update(HotelRoom obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            HotelRoom hotelRoom = GetById(Id);
            context.Remove(hotelRoom);
        }
    }
}
