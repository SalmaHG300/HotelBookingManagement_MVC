using Microsoft.AspNetCore.Mvc;
using HotelManagement_MVC.ViewModel;
using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingDiningController : Controller
    {
        IBookingDiningRepo BookingDiningRepo;
        IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        ICartRepo CartRepo;
        IDiningRepo DiningRepo;

        public BookingDiningController(IBookingDiningRepo BookingDiningRepo, IDiningRepo DiningRepo, ICartRepo cartRepo, IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.BookingDiningRepo = BookingDiningRepo;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.CartRepo = cartRepo;
            this.DiningRepo = DiningRepo;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveNew(int id, BookingDiningVM bookingDiningVM)
        {
            var dining = BookingDiningRepo.GetDiningById(id);
            if (dining == null)
            {
                return RedirectToAction("GetAll", "Dining"); // Or handle the error as appropriate
            }

            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {

                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                var bookingDining = new BookingDining
                {
                    Date = bookingDiningVM.Date,
                    NumAdults = bookingDiningVM.NumAdults,
                    SpecialRequest = bookingDiningVM.SpecialRequest,
                    ApplicationUserId = userId,
                    Price = dining.Price,
                    DiningId = dining.Id
                    // Additional properties related to dining can be set here if needed
                };
                bookingDining.Price = (int)BookingDiningRepo.DuplicatePrice(bookingDining);

                var cart = CartRepo.GetCartByGuestId(userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        ApplicationUserId = userId
                    };
                    CartRepo.Insert(cart);
                }
                if (cart.BookingDinings == null)
                    cart.BookingDinings = new List<BookingDining> { bookingDining };
                else
                    cart.BookingDinings.Add(bookingDining);
                cart.ShippingPrice = (int)CartRepo.CalculateTotalPrice(cart);

                BookingDiningRepo.Insert(bookingDining);
                BookingDiningRepo.Save();
                CartRepo.Update(cart);
                CartRepo.Save();

                // Redirect to cart confirmation page
                return RedirectToAction("GetAll", "Dining", new { Id = userId });
            }
            else
            {
                return RedirectToAction("Login", "Account"); // Redirect user to login if not authenticated
            }
        }
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated == true || User.IsInRole("Admin")) //If the user is not logedin redirect the view to the login
            {

                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                BookingDining bookingDining = BookingDiningRepo.GetById(id);

                if (bookingDining == null)
                {
                    return NotFound();
                }
                else
                {
                    BookingDiningRepo.Delete(id);
                    BookingDiningRepo.Save();
                }
                var cart = CartRepo.GetCartByGuestId(userId);
                cart.ShippingPrice = (int)CartRepo.CalculateTotalPrice(cart);
                CartRepo.Update(cart);
                CartRepo.Save();
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

        [AllowAnonymous]
        //EDIT
        public IActionResult Edit(int id)
        {
            var bookingDiningEdit = BookingDiningRepo.GetById(id);

            if (bookingDiningEdit == null)
            {
                return NotFound();
            }

            var diningOptions = DiningRepo.GetAll(); // Assuming you have a DiningRepo to fetch dining options
            ViewData["DiningList"] = diningOptions;
            var viewModel = new BookingDiningEditVM
            {
                Id = bookingDiningEdit.Id,
                Date = bookingDiningEdit.Date,
                NumAdults = bookingDiningEdit.NumAdults,
                //Price = bookingDiningEdit.Price,
                SpecialRequest = bookingDiningEdit.SpecialRequest,
                DiningId = bookingDiningEdit.DiningId,
            };

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SaveEdit(BookingDiningEditVM viewModel)
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {
                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                if (ModelState.IsValid)
                {
                    var bookingDiningDb = BookingDiningRepo.GetById(viewModel.Id);

                    bookingDiningDb.Date = viewModel.Date;
                    bookingDiningDb.NumAdults = viewModel.NumAdults;
                    bookingDiningDb.SpecialRequest = viewModel.SpecialRequest;
                    bookingDiningDb.DiningId = viewModel.DiningId;

                    int newPrice = (int)BookingDiningRepo.DuplicatePrice(bookingDiningDb);
                    bookingDiningDb.Price = newPrice;

                    BookingDiningRepo.Update(bookingDiningDb);
                    BookingDiningRepo.Save();

                    var cart = CartRepo.GetCartByGuestId(userId);
                    cart.ShippingPrice = (int)CartRepo.CalculateTotalPrice(cart);
                    CartRepo.Update(cart);
                    CartRepo.Save();
                    return RedirectToAction("GetAllCart", "Cart");
                }
            }
                var diningOptions = DiningRepo.GetAll();
                ViewData["DiningList"] = diningOptions;

                return View("Edit", viewModel);
            }
            }

        }


