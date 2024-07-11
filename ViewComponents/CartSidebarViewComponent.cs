using Microsoft.AspNetCore.Mvc;
using HotelManagement_MVC.IRepository;
using NuGet.Protocol.Plugins;
using HotelManagement_MVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace HotelManagement_MVC.ViewComponents
{
    public class CartSidebarViewComponent : ViewComponent
    {
        public ICartRepo _cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartSidebarViewComponent(ICartRepo cartRepository , UserManager<ApplicationUser> userManager)
        {
            _cartRepository = cartRepository;
            this.userManager = userManager;
        }

        //public IViewComponentResult Invoke()
        //{
        //    List<Cart> cartList;
        //    if (User.Identity.IsAuthenticated != true) //If the user is not logedin redirect the view to the login
        //    {
        //         cartList = null;
        //        return View(cartList);
        //    }
        //         cartList = _cartRepository.GetAll();
        //    return View(cartList);
        //}

        //public IViewComponentResult Invoke()
        //{
        //    Cart cart;
        //    if (User.Identity.IsAuthenticated != true) //If the user is not logedin redirect the view to the login
        //    {
        //        cart = null;
        //        return View(cart);
        //    }
        //    else //If the user is not logedin redirect the view to the login
        //    {
        //        Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //        string Id = ClaimId.Value;
        //        cart = _cartRepository.GetCartByGuestId(Id);
        //        return View(cart);
        //    }

        //}
        public IViewComponentResult Invoke()
        {
            Cart cart;

            if (!User.Identity.IsAuthenticated)
            {
                cart = null;
                return View(cart);
            }
            else
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                var claimId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (claimId == null)
                {
                    cart = null;
                    return View(cart);
                }

                string id = claimId.Value;
                cart = _cartRepository.GetCartByGuestId(id);
                return View(cart);
            }
        }
        //public IViewComponentResult Invoke()
        //{
        //    Cart cart;

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        cart = null;
        //        return View(cart);
        //    }
        //    else
        //    {
        //        var claimsPrincipal = User as ClaimsPrincipal;
        //        var claimId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        //        if (claimId == null)
        //        {
        //            cart = null;
        //            return View(cart);
        //        }

        //        string id = claimId.Value;
        //        cart = _cartRepository.GetCartByGuestId(id);

        //        // Populate ViewData with image data
        //        if (cart != null && cart.BookingRooms != null)
        //        {
        //            foreach (var bookingRoom in cart.BookingRooms)
        //            {
        //                ViewData[$"RoomImage_{bookingRoom.Id}"] = bookingRoom.HotelRoom.HotelRoomType.Images;
        //            }
        //        }

        //        return View(cart);
        //    }
        //}

    }
}