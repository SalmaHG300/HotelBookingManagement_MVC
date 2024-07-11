using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;
using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HotelManagement_MVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class BookingRoomController : Controller
    {
        private readonly IBookingRoomRepo bookingRoomRepo;
        private readonly IOfferRepo offerRepo;
        private readonly IHotelRoomRepo hotelRoomRepo;
        private readonly IHotelRoomTypeRepo hotelRoomTypeRepo;
        private readonly IHotelFloorRepo hotelFloorRepo;
        private readonly ICartRepo cartRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public BookingRoomController(IBookingRoomRepo bookingRoomRepo,IOfferRepo offerRepo,
            IHotelRoomRepo hotelRoomRepo,IHotelRoomTypeRepo hotelRoomTypeRepo,IHotelFloorRepo hotelFloorRepo
            , ICartRepo cartRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.bookingRoomRepo = bookingRoomRepo;
            this.offerRepo = offerRepo;
            this.hotelRoomRepo = hotelRoomRepo;
            this.hotelRoomTypeRepo = hotelRoomTypeRepo;
            this.hotelFloorRepo = hotelFloorRepo;
            this.cartRepo = cartRepo;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult BookingRoom()
        {
            ViewData["OfferList"] = offerRepo.GetAll();
            ViewData["FloorList"] = hotelFloorRepo.GetAll();
            ViewData["RoomTypeList"] = hotelRoomTypeRepo.GetAll();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveNew(int id,BookingRoomOfferRoomTypeVM bookingRoomReq)
        {
            //var Room = bookingRoomRepo.GetRoomTypeById(id);
            //if (Room == null)
            //{
            //    return RedirectToAction("RoomTypes", "HotelRoomType"); // Or handle the error as appropriate
            //}
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {
                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (ClaimId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                string Id = ClaimId.Value;
                if (ModelState.IsValid)
                {
                    BookingRoom bookingRoom = new BookingRoom
                    {
                        ApplicationUserId = Id,
                        NumAdults = bookingRoomReq.NumAdults,
                        NumChildren = bookingRoomReq.NumChildren,
                        OfferId = bookingRoomReq.OfferId,
                        CheckInDate = bookingRoomReq.CheckInDate,
                        CheckOutDate = bookingRoomReq.CheckOutDate,
                        NumOfRooms = bookingRoomReq.NumOfRooms,
                        SpecialRequest = bookingRoomReq.SpecialRequest,
                        HotelRoom = new HotelRoom
                        {
                            HotelRoomTypeId = bookingRoomReq.HotelRoomTypeId,
                            HotelFloorId = bookingRoomReq.HotelFloorId
                        }
                    };
                    var totalPrice = bookingRoomRepo.DuplicatePrice(bookingRoom);
                    if (totalPrice != null)
                    {
                        bookingRoom.TotalPrice = (int)bookingRoomRepo.DuplicatePrice(bookingRoom);
                        var cart = cartRepo.GetCartByGuestId(Id);
                        if (cart == null)
                        {
                            cart = new Cart
                            {
                                ApplicationUserId = Id
                            };
                            cartRepo.Insert(cart);
                        }
                        if (cart.BookingRooms == null)
                            cart.BookingRooms = new List<BookingRoom> { bookingRoom };
                        else
                            cart.BookingRooms.Add(bookingRoom);
                        cart.ShippingPrice = (int)cartRepo.CalculateTotalPrice(cart);

                        bookingRoomRepo.Insert(bookingRoom);
                        bookingRoomRepo.Save();
                        cartRepo.Update(cart);
                        cartRepo.Save();

                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewData["OfferList"] = offerRepo.GetAll();
            ViewData["FloorList"] = hotelFloorRepo.GetAll();
            ViewData["RoomTypeList"] = hotelRoomTypeRepo.GetAll();
            return View("BookingRoom", bookingRoomReq);
        }
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {

                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                BookingRoom bookingRoom = bookingRoomRepo.GetById(id);

            if (bookingRoom == null)
            {
                return NotFound();
            }
            else
            {
                bookingRoomRepo.Delete(id);
                bookingRoomRepo.Save();
            }
            var cart = cartRepo.GetCartByGuestId(userId);
            cart.ShippingPrice = (int)cartRepo.CalculateTotalPrice(cart);
            cartRepo.Update(cart);
            cartRepo.Save();
                if (User.IsInRole("Admin"))
                    return RedirectToAction("GetAllCartAdmin", "Cart");
                else
                    return RedirectToAction("GetAllCart", "Cart");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
  
        public IActionResult Edit(int id)
        {
            BookingRoom bookingRoomEdit = bookingRoomRepo.GetById(id);

            if (bookingRoomEdit == null)
            {
                return NotFound();
            }

            var roomTypeId = bookingRoomEdit.HotelRoom?.HotelRoomTypeId;
            var floorId = bookingRoomEdit.HotelRoom?.HotelFloorId;

            ViewData["OfferList"] = offerRepo.GetAll();
            ViewData["FloorList"] = hotelFloorRepo.GetAll();
            ViewData["RoomTypeList"] = hotelRoomTypeRepo.GetAll();

            BookingRoomOfferRoomTypeVM bookingRoom = new BookingRoomOfferRoomTypeVM
            {
                Id = bookingRoomEdit.Id,
                NumAdults = bookingRoomEdit.NumAdults,
                HotelFloorId = floorId ?? 0, // Assuming default value if null
                NumOfRooms = bookingRoomEdit.NumOfRooms,
                NumChildren = bookingRoomEdit.NumChildren,
                OfferId = bookingRoomEdit.OfferId,
                CheckInDate = bookingRoomEdit.CheckInDate,
                CheckOutDate = bookingRoomEdit.CheckOutDate,
                SpecialRequest = bookingRoomEdit.SpecialRequest,
                HotelRoomTypeId = roomTypeId ?? 0 // Assuming default value if null
            };

            return View(bookingRoom);
        }

        [HttpPost]
        public IActionResult SaveEdit(BookingRoomOfferRoomTypeVM viewModel)
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {
                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                if (ModelState.IsValid)
                {
                    var bookingRoomDb = bookingRoomRepo.GetById(viewModel.Id);

                    bookingRoomDb.CheckInDate = viewModel.CheckInDate;
                    bookingRoomDb.CheckOutDate = viewModel.CheckOutDate;
                    bookingRoomDb.NumAdults = viewModel.NumAdults;
                    bookingRoomDb.SpecialRequest = viewModel.SpecialRequest;
                    bookingRoomDb.NumChildren = viewModel.NumChildren;
                    bookingRoomDb.OfferId = viewModel.OfferId;
                    bookingRoomDb.NumOfRooms = viewModel.NumOfRooms;

                    var hotelRoom = hotelRoomRepo.GetById(bookingRoomDb.HotelRoomId);
                    if (hotelRoom != null)
                    {
                        hotelRoom.HotelRoomTypeId = viewModel.HotelRoomTypeId;
                        hotelRoom.HotelFloorId = viewModel.HotelFloorId;
                        bookingRoomDb.HotelRoom = hotelRoom;
                    }

                    int newPrice = (int)bookingRoomRepo.DuplicatePrice(bookingRoomDb);
                    bookingRoomDb.TotalPrice = newPrice;

                    bookingRoomRepo.Update(bookingRoomDb);
                    bookingRoomRepo.Save();

                    var cart = cartRepo.GetCartByGuestId(userId);
                    cart.ShippingPrice = (int)cartRepo.CalculateTotalPrice(cart);
                    cartRepo.Update(cart);
                    cartRepo.Save();
                    return RedirectToAction("GetAllCart", "Cart");
                }
            }
            ViewData["OfferList"] = offerRepo.GetAll();
            ViewData["FloorList"] = hotelFloorRepo.GetAll();
            ViewData["RoomTypeList"] = hotelRoomTypeRepo.GetAll();

            return View("Edit", viewModel);
        }


    }
}



