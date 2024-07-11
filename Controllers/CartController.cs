using Microsoft.AspNetCore.Mvc;
using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using HotelManagement_MVC.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelManagement_MVC.Repository;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CartController : Controller
    {
        ICartRepo CartRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        IBookingDiningRepo BookingDiningRepo;
        IBookingExperienceRepo BookingExperienceRepo;
        IBookingRoomRepo BookingRoomRepo;

        public CartController(ICartRepo CartRepo, IWebHostEnvironment webHostEnvironment, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , 
            IBookingDiningRepo bookingDiningRepo , IBookingExperienceRepo bookingExperienceRepo , IBookingRoomRepo bookingRoomRepo)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.CartRepo = CartRepo;
            this.BookingDiningRepo = bookingDiningRepo;
            this.BookingExperienceRepo = bookingExperienceRepo;
            this.BookingRoomRepo = bookingRoomRepo;
        }

        public IActionResult Index(string search, int pg = 1)
        {
            List<Cart> cartList ;
            if (!string.IsNullOrEmpty(search))
            {
                cartList = CartRepo.Search(search);
            }
            else
            {
                cartList = CartRepo.GetAll();
                const int pageSize = 5;
                if (pg < 1) pg = 1;
                int recsCount = cartList.Count();
                Pager pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = cartList.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return View(data);
            }
            return View(cartList);
        }

        [AllowAnonymous]
        public IActionResult GetAllCart()
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {
                Cart cart;

                    var claimsPrincipal = User as ClaimsPrincipal;
                    var claimId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                    if (claimId == null)
                    {
                        cart = null;
                        return View(cart);
                    }
                    string id = claimId.Value;
                    cart = CartRepo.GetCartByGuestId(id);
                    return View(cart);
                
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult GetAllCartAdmin()
        {

            var CartList = CartRepo.GetAll();
            if (CartList != null)
            {
                return View(CartList);
            }
            return View(CartList);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmCart()
        {
            if (User.Identity.IsAuthenticated == true) //If the user is not logedin redirect the view to the login
            {

                Claim ClaimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string Id = ClaimId.Value;
                Cart cart = CartRepo.GetCartByGuestId(Id);
                if (cart == null)
                {
                    return NotFound(); // Handle case where cart is not found
                }

                // Call CreatePaymentIntent method
                var paymentUrl = await CreatePaymentIntent(cart);

                if (!string.IsNullOrEmpty(paymentUrl))
                {
                    cart.paymentStatus = Enums.PaymentStatus.Pending;
                    // Redirect user to payment page
                    return Redirect(paymentUrl);

                }
                else
                {
                    cart.paymentStatus = Enums.PaymentStatus.Declined;
                    // Handle payment creation failure
                    return RedirectToAction("PaymentError");
                }
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // This method creates the payment intent
        [AllowAnonymous]
        private async Task<string> CreatePaymentIntent(Cart cart)
        {
            var paymobApiKey = _configuration["Paymob:ApiKey"];
            var paymobBaseUrl = "https://accept.paymobsolutions.com/api";

            using (var client = new HttpClient())
            {
                // Step 1: Authentication
                var authContent = new StringContent(
                    $"{{\"api_key\":\"{paymobApiKey}\"}}",
                    Encoding.UTF8,
                    "application/json");
                var authResponse = await client.PostAsync($"{paymobBaseUrl}/auth/tokens", authContent);
                if (!authResponse.IsSuccessStatusCode)
                {
                    // Handle authentication failure
                    return null;
                }

                var authResult = JObject.Parse(await authResponse.Content.ReadAsStringAsync());
                var token = authResult["token"].ToString();
                var amount = cart.ShippingPrice * 100; // Convert amount to cents

                // Construct the request body
                var requestBody = new
                {
                    auth_token = token,
                    delivery_needed = false,
                    amount_cents = amount, // Use the cart total as the amount
                    currency = "EGP",
                    items = new List<Object>(),
                };

                // Serialize the request body
                var requestBodyJson = JsonConvert.SerializeObject(requestBody);

                // Step 2: Create Order
                var orderContent = new StringContent(
                    requestBodyJson,
                    Encoding.UTF8,
                    "application/json");

                var orderResponse = await client.PostAsync($"{paymobBaseUrl}/ecommerce/orders", orderContent);
                if (!orderResponse.IsSuccessStatusCode)
                {
                    // Handle order creation failure
                    var errorMessage = await orderResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Order creation failed: {errorMessage}");
                    return null;
                }

                var orderResult = JObject.Parse(await orderResponse.Content.ReadAsStringAsync());
                var orderId = orderResult["id"].ToString();

                // Step 3: Create Payment Key
                var paymentKeyContent = new
                {
                    auth_token = token,
                    amount_cents = amount,
                    expiration = 3600,
                    order_id = orderId,
                    billing_data = new
                    {
                        apartment = "NA",
                        email = "NA",
                        floor = "NA",
                        first_name = "NA",
                        street = "NA",
                        building = "NA",
                        phone_number = "NA",
                        shipping_method = "NA",
                        postal_code = "NA",
                        city = "NA",
                        country = "NA",
                        last_name = "NA",
                        state = "NA"
                    },
                    currency = "EGP",
                    integration_id = 4579761
                };
                var paymentKeyContentJson = JsonConvert.SerializeObject(paymentKeyContent);
                var paymentContent = new StringContent(paymentKeyContentJson,Encoding.UTF8,"application/json");

                var paymentKeyResponse = await client.PostAsync($"{paymobBaseUrl}/acceptance/payment_keys", paymentContent);
                if (!paymentKeyResponse.IsSuccessStatusCode)
                {
                    // Handle payment key creation failure
                    var errorMessage = await paymentKeyResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Payment key creation failed: {errorMessage}");
                    return null;
                }

                var paymentKeyResult = JObject.Parse(await paymentKeyResponse.Content.ReadAsStringAsync());
                var paymentKey = paymentKeyResult["token"].ToString();

                // Redirect to payment
                return $"https://accept.paymobsolutions.com/api/acceptance/iframes/847676?payment_token={paymentKey}";
            }
        }
    }
}

