using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.Repository
{
    public class HotelFloorRepo : IHotelFloorRepo
    {
        public HotelContext context;
        public HotelFloorRepo(HotelContext _context)
        {
            context = _context;
        }

        public List<HotelFloor> GetAll()
        {
           return context.HotelFloors.ToList();
        }
    }
}
