using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.Repository
{
    public class DiningRepo : IDiningRepo
    {
        public HotelContext context;
        public DiningRepo(HotelContext _context)
        {
            context = _context;
        }
        public List<Dining> GetAll()
        {
            return context.Dinings.ToList();
        }

        public Dining GetById(int Id)
        {
            return context.Dinings.FirstOrDefault(d => d.Id == Id);
        }
        public void Insert(Dining obj)
        {
            context.Add(obj);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public List<Dining> Search(string search)
        {
            return context.Dinings
                    .Where(d => d.Name.Contains(search))
                    .ToList();
        }

        public void Update(Dining obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            Dining dining = GetById(Id);
            context.Remove(dining);
        }
    }
}
