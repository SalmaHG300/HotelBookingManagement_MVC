using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace HotelManagement_MVC.Repository
{
    public class CartRepo : ICartRepo
    {
        public HotelContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CartRepo(HotelContext _context, UserManager<ApplicationUser> userManager)
        {
            context = _context;
            this.userManager = userManager;
        }
        public List<Cart> GetAll()
        {
            return context.Carts.Include(c => c.BookingDinings).ThenInclude(bd => bd.Dining)
                .Include(c => c.BookingRooms).ThenInclude(br => br.HotelRoom).ThenInclude(ht => ht.HotelRoomType)
                .Include(c => c.BookingExperiences).ThenInclude(be => be.Experience).Include(u=>u.ApplicationUser).ToList();
        }
        public List<Cart> Search(string search)
        {
            return context.Carts.Include(u => u.ApplicationUser)
                    .Where(i => i.ApplicationUser.UserName.Contains(search))
                    .ToList();
        }
        public Cart GetById(int Id)
        {
            return context.Carts.FirstOrDefault(d => d.Id == Id);
        }


        public void Insert(Cart obj)
        {
            context.Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Cart obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            Cart cart = GetById(Id);
            context.Remove(cart);
        }
        public Cart GetCartByGuestId(string id)
        {
            return context.Carts
                .Include(c => c.BookingDinings).ThenInclude(bd=> bd.Dining)
                .Include(c => c.BookingRooms).ThenInclude(br => br.HotelRoom).ThenInclude(hr=>hr.HotelRoomType)
                .Include(c => c.BookingExperiences).ThenInclude(be=> be.Experience)
                .FirstOrDefault(c => c.ApplicationUserId == id);
        }

        public int? CalculateTotalPrice(Cart obj)
        {
            int? totalPrice = 0;

            if (obj.BookingDinings != null)
            {
                totalPrice += obj.BookingDinings.Sum(bd => bd.Price);
            }

            if (obj.BookingRooms != null)
            {
                totalPrice += obj.BookingRooms.Sum(br => br.TotalPrice);
            }

            if (obj.BookingExperiences != null)
            {
                totalPrice += obj.BookingExperiences.Sum(be => be.Price);
            }

            return totalPrice;
        }
        //public Cart GetCartByBookingDiningID (int bookingid)
        //{
        //    var cart = context.Carts.FirstOrDefault(c=>c.BookingDinings in  bookingid);
        //}
      
    }
}
