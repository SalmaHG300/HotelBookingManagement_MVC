using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.ViewModel;

namespace HotelManagement_MVC.Repository
{
    public class ExperienceTypeRepo:IExperienceTypeRepo
    {
        public HotelContext context;
        public ExperienceTypeRepo(HotelContext _context)
        {
            context = _context;
        }
        public List<ExperienceType> GetAll()
        {
            return context.ExperienceTypes.ToList();
        }
        
    }
}
