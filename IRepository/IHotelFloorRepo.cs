using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IHotelFloorRepo
    {
        List<HotelFloor> GetAll();
    }
}
