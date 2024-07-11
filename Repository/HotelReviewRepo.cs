using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.Repository
{
    public class HotelReviewRepo : IHotelReviewRepo
    {
        public HotelContext context;
        public HotelReviewRepo(HotelContext _context)
        {
            context = _context;
        }
    }
}
