using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IExperienceRepo
    {
        List<Experience> GetAll();
        Experience GetById(int Id);
        void Update(Experience obj);
        void Delete(int Id);
        void Insert(Experience obj);
        void Save();
        public List<Experience> Search(string search);
    }
}
