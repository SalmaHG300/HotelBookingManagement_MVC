using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;

namespace HotelManagement_MVC.IRepository
{
    public interface IDiningRepo
    {
        public List<Dining> GetAll();
        public Dining GetById(int Id);
        public void Insert(Dining obj);
        public void Save();
        public List<Dining> Search(string search);
        public void Update(Dining obj);
        public void Delete(int Id);
    }
}
