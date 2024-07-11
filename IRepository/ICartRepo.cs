using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface ICartRepo
    {
        public List<Cart> GetAll();
        public Cart GetById(int Id);
        public void Insert(Cart obj);
        public void Save();
        public void Update(Cart obj);
        public void Delete(int Id);
        public Cart GetCartByGuestId(string id);
        public int? CalculateTotalPrice(Cart obj);
        public List<Cart> Search(string search);
    }
}
