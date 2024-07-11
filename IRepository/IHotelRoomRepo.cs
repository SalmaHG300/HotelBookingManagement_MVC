using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IHotelRoomRepo
    {
        List<HotelRoom> GetAll();
        HotelRoom GetById(int Id);
        void Update(HotelRoom obj);
        void Delete(int Id);
        void Insert(HotelRoom obj);
        void Save();
        public List<HotelRoom> Search(string search);
    }
}
