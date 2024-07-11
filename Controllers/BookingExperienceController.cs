using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;
using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookingExperienceController : Controller
    { 
    IBookingExperienceRepo BookingExperienceRepo;
    IWebHostEnvironment webHostEnvironment;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    ICartRepo CartRepo;

    public BookingExperienceController(IBookingExperienceRepo BookingExperienceRepo, ICartRepo cartRepo, IWebHostEnvironment webHostEnvironment,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        this.BookingExperienceRepo = BookingExperienceRepo;
        this.webHostEnvironment = webHostEnvironment;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.CartRepo = cartRepo;
    }
        [AllowAnonymous]
    [HttpPost]
    public IActionResult SaveNew(int id, BookingExperienceVM bookingExperienceVM) //CREATE BOOKING VIEWMODEL
    {
        var Experience = BookingExperienceRepo.GetExperienceById(id);
        if (Experience == null)
        {
            return RedirectToAction("Experiences", "Experience"); // Or handle the error as appropriate
        }

        if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
        {

            Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = ClaimId.Value;

            var bookingExperience = new BookingExperience
            {
                Date = bookingExperienceVM.Date,
                NumAdults = bookingExperienceVM.NumAdults,
                ApplicationUserId = userId,
                Price = Experience.Price,
                ExperienceId = Experience.Id 
            };
                bookingExperience.Price = (int)BookingExperienceRepo.DuplicatePrice(bookingExperience);

            var cart = CartRepo.GetCartByGuestId(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId
                };
                CartRepo.Insert(cart);
            }
            if (cart.BookingExperiences == null)
                cart.BookingExperiences = new List<BookingExperience> { bookingExperience };
            else
                cart.BookingExperiences.Add(bookingExperience);
            cart.ShippingPrice = (int)CartRepo.CalculateTotalPrice(cart); //MAY NEED TO PARSE STRING TO INT

                BookingExperienceRepo.Insert(bookingExperience);
                BookingExperienceRepo.Save();
            CartRepo.Update(cart);
            CartRepo.Save();


            // Redirect to cart confirmation page
            return RedirectToAction("Experiences", "Experience");
        }
        else
        {
            return RedirectToAction("Login", "Account"); // Redirect user to login if not authenticated
        }

    }
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {

                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                BookingExperience bookingExperience = BookingExperienceRepo.GetById(id);

            if (bookingExperience == null)
            {
                return NotFound();
            }
            else
            {
                BookingExperienceRepo.Delete(id);
                BookingExperienceRepo.Save();
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
        //EDIT
        [AllowAnonymous]
        public IActionResult Edit(int id)
        {
            var bookingExperienceEdit = BookingExperienceRepo.GetById(id);

            if (bookingExperienceEdit == null)
            {
                return NotFound();
            }

           
            var viewModel = new BookingExperienceEditVM
            {
                Date = bookingExperienceEdit.Date,
                NumAdults = bookingExperienceEdit.NumAdults,
                SpecialRequest = bookingExperienceEdit.SpecialRequest,
                Id=bookingExperienceEdit.Id
            };

            return View(viewModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SaveEdit(BookingExperienceEditVM viewModel)
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {
                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userId = ClaimId.Value;

                if (ModelState.IsValid)
                {
                    var bookingexperienceDb = BookingExperienceRepo.GetById(viewModel.Id);

                    bookingexperienceDb.Date = viewModel.Date;
                    bookingexperienceDb.NumAdults = viewModel.NumAdults;
                    bookingexperienceDb.SpecialRequest = viewModel.SpecialRequest;

                    int newPrice = (int)BookingExperienceRepo.DuplicatePrice(bookingexperienceDb);
                    bookingexperienceDb.Price = newPrice;

                    BookingExperienceRepo.Update(bookingexperienceDb);
                    BookingExperienceRepo.Save();

                    var cart = CartRepo.GetCartByGuestId(userId);
                    cart.ShippingPrice = (int)CartRepo.CalculateTotalPrice(cart);
                    CartRepo.Update(cart);
                    CartRepo.Save();
                    return RedirectToAction("GetAllCart", "Cart");
                }
            }

            return View("Edit", viewModel);
        }
       
    }
}
