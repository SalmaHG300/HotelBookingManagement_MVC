using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IHotelRoomTypeRepo
    {
        List<HotelRoomType> GetAll();
        HotelRoomType GetById(int Id);
        void Update(HotelRoomType obj);
        void Delete(int Id);
        void Insert(HotelRoomType obj);
        void Save();
        public List<HotelRoomType> Search(string search);
            
    }
}
