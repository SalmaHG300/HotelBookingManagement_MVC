using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.Repository

{
    public class AdminRepo : IAdminRepo
    {
        public HotelContext context;
        public AdminRepo(HotelContext _context)
        {
            context = _context;
        }
    }
}
