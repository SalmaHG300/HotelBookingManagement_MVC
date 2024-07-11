using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]//cookie and assigned to role admin
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> RoleManager)
        {
            roleManager = RoleManager;
        }
        [HttpGet]
        public IActionResult Create()
        {         
            return View();        
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleFReq)
        {
            if (ModelState.IsValid)
            {
                //create role database
                IdentityRole role = new IdentityRole() { Name = roleFReq.RoleName };
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View();
        }
    }
}
