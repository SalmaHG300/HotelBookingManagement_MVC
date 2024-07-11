using HotelManagement_MVC.IRepository;
using HotelManagement_MVC.Models;
using HotelManagement_MVC.ViewModel;
using HotelManagement_MVC.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagement_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExperienceController : Controller
    {
        private readonly IExperienceRepo ExperienceRepo;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IExperienceTypeRepo experienceTypeRepo;

        public ExperienceController(IExperienceRepo _ExperienceRepo, IExperienceTypeRepo _ExperienceTypeRepo, IWebHostEnvironment _WebHostEnvironment)
        {
            ExperienceRepo = _ExperienceRepo;
            WebHostEnvironment = _WebHostEnvironment;
            experienceTypeRepo = _ExperienceTypeRepo;
        }

        //index
        public IActionResult Index(string search)
        {
            List<Experience> ExperienceList;

            if (!string.IsNullOrEmpty(search))
            {
                ExperienceList = ExperienceRepo.Search(search);
            }
            else
            {
                ExperienceList = ExperienceRepo.GetAll();
            }

            return View("Index", ExperienceList);
        }

        [AllowAnonymous]
        public IActionResult Experiences(string search)
        {
            List<Experience> ExperienceList;
            if (!string.IsNullOrEmpty(search))
            {
                ExperienceList = ExperienceRepo.Search(search);
            }
            else
            {
                ExperienceList = ExperienceRepo.GetAll();
            }


            return View("Experiences", ExperienceList);
        }

        [AllowAnonymous]
        //details
        public IActionResult Details(int Id)
        {
            Experience ExperienceDetails = ExperienceRepo.GetById(Id);
            return View("Details", ExperienceDetails);
        }
        [HttpGet]

        //new
        public IActionResult New()
        {
            List<ExperienceType> ExperiencetypeList = experienceTypeRepo.GetAll();
            ExperienceWithTypesViewModel experienceVM = new ExperienceWithTypesViewModel();
            experienceVM.Type = ExperiencetypeList;
            return View("New", experienceVM);
        }

        [HttpPost]
        public IActionResult SaveNew(ExperienceWithTypesViewModel experiencenew, IFormFile FileImage, IFormFile FileCoverImage)
        {
            List<ExperienceType> ExperiencetypeList = experienceTypeRepo.GetAll();
            experiencenew.Type = ExperiencetypeList;

            if (ModelState.IsValid)
            {
                Experience ExperienceDb = new Experience();
                ExperienceDb.Price = experiencenew.Price;
                ExperienceDb.Id = experiencenew.ExperienceId;
                ExperienceDb.Name = experiencenew.ExperienceName;
                ExperienceDb.Description = experiencenew.Description;
                ExperienceDb.Duration = experiencenew.Duration;
                ExperienceDb.instructions = experiencenew.instructions;
                ExperienceDb.Requirements = experiencenew.Requirements;
                ExperienceDb.TypeId = experiencenew.TypeId;

                if (FileImage != null && FileImage.Length > 0)
                {
                    var imageFilePath = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Experience/", FileImage.FileName);

                    using (var stream = new FileStream(imageFilePath, FileMode.Create))
                    {
                        FileImage.CopyTo(stream);
                    }

                    ExperienceDb.Image = FileImage.FileName;
                }

                if (FileCoverImage != null && FileCoverImage.Length > 0)
                {
                    var coverImageFilePath = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Experience/", FileCoverImage.FileName);

                    using (var stream = new FileStream(coverImageFilePath, FileMode.Create))
                    {
                        FileCoverImage.CopyTo(stream);
                    }

                    ExperienceDb.CoverImage = FileCoverImage.FileName;
                }

                ExperienceRepo.Insert(ExperienceDb);
                ExperienceRepo.Save();

                return RedirectToAction("Index", "Experience");
            }

            return View("New", experiencenew);
        }

            //edit
            public IActionResult Edit(int id)
        {
            var ExperienceEdit = ExperienceRepo.GetById(id);

            if (ExperienceEdit == null)
            {
                return NotFound();
            }

            var ExperiencetypeList = experienceTypeRepo.GetAll();
            var experienceVM = new ExperienceWithTypesViewModel
            {
                Type = ExperiencetypeList,
                Price = ExperienceEdit.Price,
                Image = ExperienceEdit.Image,
                CoverImage = ExperienceEdit.CoverImage,
                ExperienceName = ExperienceEdit.Name,
                Description = ExperienceEdit.Description,
                Requirements = ExperienceEdit.Requirements,
                instructions = ExperienceEdit.instructions,
                Duration = ExperienceEdit.Duration,
                TypeId = ExperienceEdit.TypeId,
                ExperienceId = ExperienceEdit.Id
            };

            return View("Edit", experienceVM);
        }

        [HttpPost]
        public IActionResult SaveEdit(ExperienceWithTypesViewModel ExperienceReq, IFormFile? FileImage, IFormFile? FileCoverImage)
        {

            if (ModelState.IsValid)
            {
                Experience ExperienceDb = ExperienceRepo.GetById(ExperienceReq.ExperienceId);

                ExperienceDb.TypeId = ExperienceReq.TypeId;
                ExperienceDb.Price = ExperienceReq.Price;
                ExperienceDb.Name = ExperienceReq.ExperienceName;
                ExperienceDb.Description = ExperienceReq.Description;
                ExperienceDb.Duration = ExperienceReq.Duration;
                //image
                if (FileImage != null && FileImage.Length > 0)
                {
                    var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Experience/", FileImage.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileImage.CopyTo(stream);

                    }

                    ExperienceReq.Image = FileImage.FileName;

                    ExperienceDb.Image = ExperienceReq.Image;
                }
                else
                {
                    ExperienceReq.Image = ExperienceDb.Image;
                }
                //cover image
                if (FileCoverImage != null && FileCoverImage.Length > 0)
                {
                    var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Experience/", FileCoverImage.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FileCoverImage.CopyTo(stream);

                    }

                    ExperienceReq.CoverImage = FileCoverImage.FileName;

                    ExperienceDb.CoverImage = ExperienceReq.CoverImage;
                }
                else
                {
                    ExperienceReq.CoverImage = ExperienceDb.CoverImage;
                }
                ExperienceRepo.Update(ExperienceDb);
                ExperienceRepo.Save();
                return RedirectToAction("Index");
            }
            List<ExperienceType> ExperiencetypeList = experienceTypeRepo.GetAll();
            ExperienceReq.Type = ExperiencetypeList;
            return View("Edit", ExperienceReq);
        }

        //delete
        public IActionResult Delete(int id)
        {
            Experience ExperienceDelete = ExperienceRepo.GetById(id);

            if (ExperienceDelete == null)
            {
                return NotFound(); 
            }
            return View("Delete", ExperienceDelete);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            Experience experience = ExperienceRepo.GetById(id);

            if (experience == null)
            {
                return NotFound(); 
            }

            ExperienceRepo.Delete(experience.Id);
            ExperienceRepo.Save();

            return RedirectToAction("Index", "Experience");
        }
    }
}
