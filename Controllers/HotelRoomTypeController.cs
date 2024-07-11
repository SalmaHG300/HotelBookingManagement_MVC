using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HotelRoomTypeController : Controller
    {
      
        private readonly IHotelRoomTypeRepo HotelRoomTypeRepo;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HotelRoomTypeController(IHotelRoomTypeRepo HotelRoomTypeRepo, IWebHostEnvironment webHostEnvironment)
        {
            this.HotelRoomTypeRepo = HotelRoomTypeRepo;
            this.webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public IActionResult RoomTypes(string search, int pg = 1)
        {
            List<HotelRoomType> HotRoomTypList;
            if (!string.IsNullOrEmpty(search))
            {
                HotRoomTypList = HotelRoomTypeRepo.Search(search);
            }
            else
            {
                HotRoomTypList = HotelRoomTypeRepo.GetAll();
                const int pageSize = 5;
                if (pg < 1) pg = 1;
                int recsCount = HotRoomTypList.Count();
                Pager pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = HotRoomTypList.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return View(data);
            }
            return View("RoomTypes", HotRoomTypList);
        }

        public IActionResult Index(string search, int pg = 1) 
        {
            List<HotelRoomType> HotRoomTypList;
            if(!string.IsNullOrEmpty(search))
            {
                HotRoomTypList = HotelRoomTypeRepo.Search(search);
            }
            else
            {
                HotRoomTypList= HotelRoomTypeRepo.GetAll();
                const int pageSize = 5;
                if (pg < 1) pg = 1;
                int recsCount = HotRoomTypList.Count();
                Pager pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = HotRoomTypList.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return View(data);
            }
            return View("Index",HotRoomTypList);
        }

        [AllowAnonymous]
        public IActionResult Details(int Id)
        {
            HotelRoomType hotelRoomType = HotelRoomTypeRepo.GetById(Id);
            return View("Details",hotelRoomType);
        }

        [HttpGet]
        public IActionResult New()
        {         
            HotelRoomType hotelRoomType = new HotelRoomType();

            return View("New", hotelRoomType);
        }


        [HttpPost]
        public IActionResult SaveNew(HotelRoomType roomType, IFormFile FileImages)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (FileImages != null && FileImages.Length > 0)
                {                
                    // Path to save the file in the wwwroot/uploads directory
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath,"Images/RoomType/", FileImages.FileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImages.CopyTo(stream);
                    }

                    // Set the file path to the Images property of the roomType object
                    roomType.Images =  FileImages.FileName;
                }

                // Save other room details to the database
                // Assuming you're using Entity Framework Core, add and save the roomType object
                HotelRoomTypeRepo.Insert(roomType);
                HotelRoomTypeRepo.Save();

                return RedirectToAction("Index","HotelRoomType");
            }

            return View("New", roomType);
        }

        public IActionResult Edit(int id)
        {
            HotelRoomType RoomType = HotelRoomTypeRepo.GetById(id);
            return View("Edit", RoomType);
        }

        [HttpPost]
        public IActionResult SaveEdit(HotelRoomType hotelRoomTypeReq, IFormFile? FileImages)
        {
            if (ModelState.IsValid)
            {
                HotelRoomType hotelRoomTypeDb = HotelRoomTypeRepo.GetById(hotelRoomTypeReq.Id);
                hotelRoomTypeDb.Name = hotelRoomTypeReq.Name;
                hotelRoomTypeDb.Area = hotelRoomTypeReq.Area;
                hotelRoomTypeDb.Price = hotelRoomTypeReq.Price;
                hotelRoomTypeDb.MaxGuestCount=hotelRoomTypeReq.MaxGuestCount;
                hotelRoomTypeDb.UniqueFeatures = hotelRoomTypeReq.UniqueFeatures;
                hotelRoomTypeDb.BathCount = hotelRoomTypeReq.BathCount;
                hotelRoomTypeDb.BedCount = hotelRoomTypeReq.BedCount;
                hotelRoomTypeDb.BedType = hotelRoomTypeReq.BedType;
                hotelRoomTypeDb.Decor = hotelRoomTypeReq.Decor;
                hotelRoomTypeDb.Description = hotelRoomTypeReq.Description;
                hotelRoomTypeDb.View = hotelRoomTypeReq.View;

                //ViewData["ImageFileName"] = hotelRoomTypeDb.Images;

                if (FileImages != null && FileImages.Length > 0)
                {
                    // Path to save the file in the wwwroot/uploads directory
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/RoomType/", FileImages.FileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImages.CopyTo(stream);

                    }

                    // Set the file path to the Images property of the roomType object
                    hotelRoomTypeReq.Images = FileImages.FileName;

                    hotelRoomTypeDb.Images = hotelRoomTypeReq.Images;
                }
                else
                {
                    hotelRoomTypeReq.Images = hotelRoomTypeDb.Images;
                }

                HotelRoomTypeRepo.Update(hotelRoomTypeDb);
                HotelRoomTypeRepo.Save();
                return RedirectToAction("Index", "HotelRoomType");
            }
            return View("Edit", hotelRoomTypeReq);
        }

        public IActionResult Delete(int id)
        {
            HotelRoomType RoomType = HotelRoomTypeRepo.GetById(id);

            if (RoomType == null)
            {
                // Instructor with the specified ID was not found
                return NotFound(); // Return a 404 Not Found status code
            }
            return View("Delete", RoomType);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            HotelRoomType RoomType = HotelRoomTypeRepo.GetById(id);

            if (RoomType == null)
            {
                // RoomType with the specified ID was not found
                return NotFound(); // Return a 404 Not Found status code
            }

            // If RoomType exists, proceed with the deletion
            HotelRoomTypeRepo.Delete(RoomType.Id);
            HotelRoomTypeRepo.Save();

            return RedirectToAction("Index","HotelRoomType");
        }


    }
}
