using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;
using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        //public async Task <IActionResult> Index(string search, int pg = 1)
        //{
        //    if (string.IsNullOrEmpty(search))
        //    {
        //        var users = userManager.Users.Select(u => new UserViewModel()
        //        {
        //            Id = u.Id,
        //            FName = u.Fname,
        //            LName = u.Lname,
        //            Email = u.Email,
        //            PhoneNumber = u.PhoneNumber,
        //            //Roles = userManager.GetRolesAsync(u).Result
        //        });
        //        const int pageSize = 5;
        //        if (pg < 1) pg = 1;
        //        int recsCount = users.Count();
        //        Pager pager = new Pager(recsCount, pg, pageSize);
        //        int recSkip = (pg - 1) * pageSize;
        //        var data = users.Skip(recSkip).Take(pager.PageSize).ToList();
        //        this.ViewBag.Pager = pager;

        //        return View(data);
        //    }
        //    else
        //    {
        //       var user=await userManager.FindByNameAsync(search);
        //        var mappedUser = new UserViewModel()
        //        {
        //            Id = user.Id,
        //            FName = user.Fname,
        //            LName = user.Lname,
        //            Email = user.Email,
        //            PhoneNumber = user.PhoneNumber,
        //            //Roles = userManager.GetRolesAsync(user).Result
        //        };
        //        return View(mappedUser);
        //    }

        //}
        public async Task<IActionResult> Index(string search, int pg = 1)
        {

            IEnumerable<ApplicationUser> users;

            if (string.IsNullOrEmpty(search))
            {
                users = userManager.Users;
            }
            else
            {
                users = userManager.Users.Where(u => u.UserName.Contains(search));
            }

            var userViewModels = users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                UserName = u.UserName,
                FName = u.Fname,
                LName = u.Lname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber            
                //Roles = userManager.GetRolesAsync(u).Result
            });

            const int pageSize = 5;
            if (pg < 1) pg = 1;
            int recsCount = userViewModels.Count();
            Pager pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = userViewModels.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }

        public async Task<IActionResult> Details(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            var UserVM = new UserViewModel()
            {
                Id = user.Id,
                UserName=user.UserName,
                FName = user.Fname,
                LName = user.Lname,
                Email = user.Email,
                password = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
            };
            return View("Details", UserVM);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userVM = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FName = user.Fname,
                LName = user.Lname,
                Email = user.Email,
                password=user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
            };

            return View("Edit", userVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(UserViewModel userReq)
        {
            if (ModelState.IsValid)
            {
                var userDb = await userManager.FindByIdAsync(userReq.Id);

                if (userDb == null)
                {
                    return NotFound();
                }

                userDb.Fname = userReq.FName;
                userDb.Lname = userReq.LName;
                userDb.UserName = userReq.UserName;
                userDb.Email = userReq.Email;
                userDb.PasswordHash = userReq.password;
                userDb.PhoneNumber = userReq.PhoneNumber;
                

                var result = await userManager.UpdateAsync(userDb);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View("Edit", userReq);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var User = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FName = user.Fname,
                LName = user.Lname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View("Delete", User);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }
           var User = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FName = user.Fname,
                LName = user.Lname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View("Delete", User);
        }
    }
}
