using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;
using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelManagement_MVC.Controllers
{
    public class HomeController : Controller
    {     
        private readonly ILogger<HomeController> _logger;
        private readonly IHotelRoomTypeRepo hotelRoomTypeRepo;
        private readonly IBookingDiningRepo bookingDiningRepo;
        private readonly IBookingExperienceRepo bookingExperienceRepo;
        private readonly IBookingRoomRepo bookingRoomRepo;

        public HomeController(ILogger<HomeController> logger,IHotelRoomTypeRepo hotelRoomTypeRepo,IBookingDiningRepo bookingDiningRepo,IBookingExperienceRepo bookingExperienceRepo,IBookingRoomRepo bookingRoomRepo)
        {
            _logger = logger;
            this.hotelRoomTypeRepo = hotelRoomTypeRepo;
            this.bookingDiningRepo = bookingDiningRepo;
            this.bookingExperienceRepo = bookingExperienceRepo;
            this.bookingRoomRepo = bookingRoomRepo;
        }

        public IActionResult Index()
        {

            List<HotelRoomType> HotRoomTypList=hotelRoomTypeRepo.GetAll();
            return View(HotRoomTypList.GetRange(2,3));
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult AboutUs()
		{
			return View();
		}
        //public IActionResult Calendar()
        //{
        //    AllBookingEventsViewModel allBookingEventsViewModel = new AllBookingEventsViewModel();
        //    allBookingEventsViewModel.Dining = diningRepo.GetAll();
        //    allBookingEventsViewModel.Experience = experienceRepo.GetAll();
        //    allBookingEventsViewModel.HotelRoomType = hotelRoomTypeRepo.GetAll();
        //    return View(allBookingEventsViewModel);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Calendar()
        {
            AllBookingEventsViewModel allBookingEventsViewModel = new AllBookingEventsViewModel();
            allBookingEventsViewModel.Dining = bookingDiningRepo.GetAll();
            allBookingEventsViewModel.Experience = bookingExperienceRepo.GetAll();
            allBookingEventsViewModel.HotelRoomType = bookingRoomRepo.GetAll();

            var events = new List<object>();

            // Add Hotel Room Types
            foreach (var room in allBookingEventsViewModel.HotelRoomType)
            {
                events.Add(new
                {
                    title = "Room: " + room.HotelRoom.HotelRoomType.Name,
                    start = room.CheckInDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = room.CheckOutDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    className = "bg-primary"
                });
            }

            // Add Dining
            foreach (var dining in allBookingEventsViewModel.Dining)
            {
                events.Add(new
                {
                    title = "Dining: " + dining.Dining.Name,
                    start = dining.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = dining.Date.ToString("yyyy-MM-ddTHH:mm:ss"),

                    className = "bg-danger"
                });
            }

            // Add Experience
            foreach (var experience in allBookingEventsViewModel.Experience)
            {
                events.Add(new
                {
                    title = "Experience: " + experience.Experience.Name,
                    start = experience.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = experience.Date.ToString("yyyy-MM-ddTHH:mm:ss"),
                    className = "bg-success"
                });
            }

            ViewBag.Events = events;

            return View(allBookingEventsViewModel);
        }
        public IActionResult BookingNow()
        {
            return View();
        }

    }
}
