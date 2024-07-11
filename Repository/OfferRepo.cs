using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;

namespace HotelManagement_MVC.Repository
{
    public class OfferRepo : IOfferRepo
    {
        public HotelContext context;
        public OfferRepo(HotelContext _context)
        {
            context = _context;
        }

        public List<Offer> GetAll()
        {
            return context.Offers.ToList();
        }

        public Offer GetById(int Id)
        {
            return context.Offers.FirstOrDefault(r => r.Id == Id);
        }


        public void Insert(Offer obj)
        {
            context.Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Offer> Search(string search)
        {
            return context.Offers
                    .Where(i => i.OfferName.Contains(search))
                    .ToList();
        }

        public void Update(Offer obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            Offer offer = GetById(Id);
            context.Remove(offer);
        }
    }
}
