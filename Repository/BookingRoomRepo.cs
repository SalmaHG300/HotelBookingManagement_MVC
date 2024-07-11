using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_MVC.Repository
{
    public class BookingRoomRepo : IBookingRoomRepo
    {
        public HotelContext context;
        public BookingRoomRepo(HotelContext _context)
        {
            context = _context;
        }
        public List<BookingRoom> GetAll()
        {
            return context.BookingRooms.Include(a => a.ApplicationUser).Include(h => h.HotelRoom).ThenInclude(c =>c.HotelRoomType).Include(o=>o.Offer).ToList();
        }

        public BookingRoom GetById(int Id)
        {
            return context.BookingRooms
                .Include(br => br.HotelRoom)
                .ThenInclude(hr => hr.HotelRoomType)
                .Include(br => br.HotelRoom)
                .ThenInclude(hr => hr.HotelFloor)
                .FirstOrDefault(d => d.Id == Id);
        }



        public void Insert(BookingRoom obj)
        {
            context.Add(obj);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        //public List<BookingRoom> Search(string search)
        //{
        //    return context.BookingRooms
        //            .Where(d => d.GuestId == int.Parse(search))
        //            .ToList();
        //}

        public void Update(BookingRoom obj)
        {
            context.Update(obj);
        }

        public void Delete(int Id)
        {
            BookingRoom bookingRoom = GetById(Id);
            context.Remove(bookingRoom);
        }

        public HotelRoomType GetRoomTypeById(int Id)
        {
            return context.HotelRoomTypes.FirstOrDefault(d => d.Id == Id);
        }
        public int? DuplicatePrice(BookingRoom bookingRoom)
        {
            if (bookingRoom == null || bookingRoom.CheckInDate == default || bookingRoom.CheckOutDate == default || bookingRoom.HotelRoomId ==null)
            {
                return null;
            }

            DateTime checkinDate = bookingRoom.CheckInDate;
            DateTime checkoutDate = bookingRoom.CheckOutDate;

            // Ensure checkoutDate is after checkinDate
            if (checkoutDate <= checkinDate)
            {
                return null;
            }

            // Calculate the number of days
            int numberOfDays = (checkoutDate - checkinDate).Days;

            var roomType = context.HotelRoomTypes.FirstOrDefault(d => d.Id == bookingRoom.HotelRoom.HotelRoomTypeId);
            if (roomType != null)
            {
                var offer = context.Offers.FirstOrDefault(d => d.Id == bookingRoom.OfferId);
                if (offer != null)
                {
                    return (roomType.Price + offer.OfferPrice) * bookingRoom.NumOfRooms * numberOfDays;
                }
                else
                {
                    return roomType.Price * bookingRoom.NumOfRooms * numberOfDays;
                }
            }
            return null;
        }
        public int? DuplicatePriceEdit(BookingRoom bookingRoom)
        {
            if (bookingRoom == null || bookingRoom.CheckInDate == default || bookingRoom.CheckOutDate == default || bookingRoom.HotelRoomId == null)
            {
                return null;
            }

            DateTime checkinDate = bookingRoom.CheckInDate;
            DateTime checkoutDate = bookingRoom.CheckOutDate;

            // Ensure checkoutDate is after checkinDate
            if (checkoutDate <= checkinDate)
            {
                return null;
            }

            // Calculate the number of days
            int numberOfDays = (checkoutDate - checkinDate).Days;

            var roomType = context.HotelRoomTypes.FirstOrDefault(d => d.Id == bookingRoom.HotelRoom.HotelRoomTypeId);
            if (roomType != null)
            {
                var offer = context.Offers.FirstOrDefault(d => d.Id == bookingRoom.OfferId);
                if (offer != null)
                {
                    return (roomType.Price + offer.OfferPrice) * bookingRoom.NumOfRooms * numberOfDays;
                }
                else
                {
                    return roomType.Price * bookingRoom.NumOfRooms * numberOfDays;
                }
            }
            return null;
        }
    }
}
