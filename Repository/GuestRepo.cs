using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.Repository
{
    public class GuestRepo : IGuestRepo
    {
        public HotelContext context;
        public GuestRepo(HotelContext _context)
        {
            context = _context;
        }
    }
}
