using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;
using HotelManagement_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiningController : Controller
    {
        IDiningRepo DiningRepo;
        IWebHostEnvironment webHostEnvironment;

        public DiningController(IDiningRepo DiningRepo, IWebHostEnvironment webHostEnvironment)
        {
            this.DiningRepo = DiningRepo;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string search)
        {
            List<Dining> dinings;

            if (!string.IsNullOrEmpty(search))
            {
                dinings = DiningRepo.Search(search);
            }
            else
            {
                dinings = DiningRepo.GetAll();
            }

            return View("Index", dinings);
        }
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            List<Dining> DiningList = DiningRepo.GetAll();
            return View("AllDining", DiningList);
        }
        public IActionResult Search(string searchStr)
        {
            List<Dining> list = DiningRepo.Search(searchStr);
            if (searchStr != null)
            {
                List<Dining> newList = [];
                foreach (var item in list)
                {
                    if (item.Name.ToLower().Contains(searchStr.ToLower()))
                    {
                        newList.Add(item);
                    }
                }//Need To make this search view
                return View("Search", newList);
            }
            return RedirectToAction("AllDining");
        }
        [AllowAnonymous]
        public IActionResult Details(int Id,BookingDiningVM bookingDiningVM)
        {
            Dining diningFromDB = DiningRepo.GetById(Id);
            bookingDiningVM = new BookingDiningVM()
            {
                DiningId = diningFromDB.Id,
                Name = diningFromDB.Name,
                Duration = diningFromDB.Duration,
                Description = diningFromDB.Description,
                TimeOfDay = diningFromDB.TimeOfDay,
                Images = diningFromDB.Images,
                Price = diningFromDB.Price
            };
            return View("DiningDetails", bookingDiningVM);
        }
        //delete
        public IActionResult Delete(int id)
        {
            Dining DiningDelete = DiningRepo.GetById(id);

            if (DiningDelete == null)
            {
                return NotFound();
            }
            return View("Delete", DiningDelete);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            Dining Dining = DiningRepo.GetById(id);

            if (Dining == null)
            {
                return NotFound();
            }

            DiningRepo.Delete(Dining.Id);
            DiningRepo.Save();

            return RedirectToAction("GetAll", "Dining");
        }

        //EDIT
        public IActionResult Edit(int id)
        {
            var DiningEdit = DiningRepo.GetById(id);

            if (DiningEdit == null)
            {
                return NotFound();
            }
            var DiningNew = new Dining
            {
                Name = DiningEdit.Name,
                Price = DiningEdit.Price,
                Images = DiningEdit.Images,
                TimeOfDay = DiningEdit.TimeOfDay,
                Description = DiningEdit.Description,
                Duration = DiningEdit.Duration,
            };

            return View("Edit", DiningNew);
        }

        [HttpPost]
        public IActionResult SaveEdit(Dining DiningNew, IFormFile? FileImage)
        {

            if (ModelState.IsValid)
            {
                Dining DiningDb = DiningRepo.GetById(DiningNew.Id);
                
                DiningDb.Price = DiningNew.Price;
                DiningDb.Name = DiningNew.Name;
                DiningDb.Description = DiningNew.Description;
                DiningDb.Duration = DiningNew.Duration;
                DiningDb.TimeOfDay = DiningNew.TimeOfDay;
                
                //image
                if (FileImage != null && FileImage.Length > 0)
                {
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/Dining/", FileImage.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImage.CopyTo(stream);

                    }

                    DiningNew.Images = FileImage.FileName;

                    DiningDb.Images = DiningNew.Images;
                }
                else
                {
                    DiningNew.Images = DiningDb.Images;
                }

                DiningRepo.Update(DiningDb);
                DiningRepo.Save();
                return RedirectToAction("GetAll");
            }
            return View("Edit", DiningNew);
        }
        [HttpGet]
        public IActionResult New()
        {
            Dining Dining = new Dining();

            return View("New", Dining);
        }


        [HttpPost]
        public IActionResult SaveNew(Dining dining, IFormFile FileImages)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (FileImages != null && FileImages.Length > 0)
                {
                    // Path to save the file in the wwwroot/uploads directory
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/Dining/", FileImages.FileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImages.CopyTo(stream);
                    }

                    // Set the file path to the Images property of the roomType object
                    dining.Images = FileImages.FileName;
                }

                // Save other room details to the database
                // Assuming you're using Entity Framework Core, add and save the roomType object
                DiningRepo.Insert(dining);
                DiningRepo.Save();

                return RedirectToAction("GetAll", "Dining");
            }

            return View("New", dining);
        }
    }
}