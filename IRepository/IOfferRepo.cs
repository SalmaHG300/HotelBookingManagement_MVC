using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.IRepository
{
    public interface IOfferRepo
    {
        List<Offer> GetAll();
        Offer GetById(int Id);
        void Update(Offer obj);
        void Delete(int Id);
        void Insert(Offer obj);
        void Save();
        public List<Offer> Search(string search);
    }
}
