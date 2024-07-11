using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics.Metrics;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]

    public class OfferController : Controller
    {
        private readonly IOfferRepo offerRepo;
        private readonly IWebHostEnvironment webHostEnvironment;

        public OfferController(IOfferRepo offerRepo, IWebHostEnvironment webHostEnvironment)
        {
            this.offerRepo = offerRepo;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string search)
        {
            List<Offer> offerList;
            if (!string.IsNullOrEmpty(search))
            {
                offerList = offerRepo.Search(search);
            }
            else
            {
                offerList = offerRepo.GetAll();
            }
            return View("Index", offerList);
        }

        [AllowAnonymous]
        public IActionResult Offers(string search)
        {
            List<Offer> offerList;
            if (!string.IsNullOrEmpty(search))
            {
                offerList = offerRepo.Search(search);
            }
            else
            {
                offerList = offerRepo.GetAll();
            }
            return View("Offers", offerList);
        }

        [AllowAnonymous]
        public IActionResult Details(int Id)
        {
            Offer offer = offerRepo.GetById(Id);
            return View("Details", offer);
        }

        [HttpGet]
        public IActionResult New()
        {
            Offer offer = new Offer();

            return View("New",offer);
        }

        [HttpPost]
        public IActionResult SaveNew(Offer offerReq, IFormFile FileImages)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (FileImages != null && FileImages.Length > 0)
                {
                    // Path to save the file in the wwwroot/uploads directory
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/Offer/", FileImages.FileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImages.CopyTo(stream);
                    }

                    // Set the file path to the Images property of the roomType object
                    offerReq.OfferImage = FileImages.FileName;
                }

                // Save other room details to the database
                // Assuming you're using Entity Framework Core, add and save the roomType object
                offerRepo.Insert(offerReq);
                offerRepo.Save();

                return RedirectToAction("Index", "Offer");
            }

            return View("New", offerReq);
        }

        public IActionResult Edit(int id)
        {
            Offer offerDb = offerRepo.GetById(id);
            return View("Edit", offerDb);
        }

        [HttpPost]
        public IActionResult SaveEdit(Offer offerReq, IFormFile? FileImages)
        {
            if (ModelState.IsValid)
            {
                Offer offerDb = offerRepo.GetById(offerReq.Id);
                offerDb.OfferName = offerReq.OfferName;
                offerDb.OfferPrice = offerReq.OfferPrice;
                offerDb.OfferDescription = offerReq.OfferDescription;

                if (FileImages != null && FileImages.Length > 0)
                {
                    // Path to save the file in the wwwroot/uploads directory
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/Offer/", FileImages.FileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImages.CopyTo(stream);
                    }

                    // Set the file path to the Images property of the roomType object
                    offerDb.OfferImage = FileImages.FileName;

                }
                else
                {
                    offerReq.OfferImage = offerDb.OfferImage;
                }

                offerRepo.Update(offerDb);
                offerRepo.Save();
                return RedirectToAction("Index", "Offer");
            }
            return View("Edit", offerReq);
        }

        public IActionResult Delete(int id)
        {
            Offer offer = offerRepo.GetById(id);

            if (offer == null)
            {
                // Instructor with the specified ID was not found
                return NotFound(); // Return a 404 Not Found status code
            }
            return View("Delete",offer);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            Offer offer = offerRepo.GetById(id);

            if (offer == null)
            {              
                return NotFound(); // Return a 404 Not Found status code
            }

            // If RoomType exists, proceed with the deletion
            offerRepo.Delete(offer.Id);
            offerRepo.Save();

            return RedirectToAction("Index", "Offer");
        }

    }
}
