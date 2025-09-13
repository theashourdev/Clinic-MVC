using AspNetCoreHero.ToastNotification.Abstractions;
using Cilinc_System.Models;
using Cilinc_System.Models.ViewModels;
using Cilinc_System.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cilinc_System.Controllers
{
    public class SpecialtiesController : Controller
    {
        private readonly ISpecialtiesService _specialtiesService;
        private readonly IDoctorService _doctorService;
        private readonly IImageManager _imageManager;
        private readonly INotyfService _notyf;

        public SpecialtiesController(ISpecialtiesService specialtiesService, IImageManager imageManager, IDoctorService doctorService, INotyfService notyf)
        {
            _specialtiesService = specialtiesService;
            _imageManager = imageManager;
            _doctorService = doctorService;
            _notyf = notyf;
        }

        // GET: Specialties
        public IActionResult Index()
        {
            var specialties = _specialtiesService.GetAll();
            return View(specialties);
        }

        // GET: Specialties/Details/5
        public IActionResult Details(int id)
        {
            var specialty = _specialtiesService.GetById(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return PartialView("_DetailsPartial", specialty);
        }

        // GET: Specialties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateSpecialtiesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ImageFile != null)
            {
                model.ImagePath = _imageManager.SaveImage(model.ImageFile, "specialties");
            }

            var specialty = new Specialties(model);

            _specialtiesService.Create(specialty);
            _notyf.Success("Specialty created successfully!");

            return RedirectToAction(nameof(Index));
        }

        // GET: Specialties/Edit/5
        public IActionResult Edit(int id)
        {
            var specialty = _specialtiesService.GetById(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Specialties model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = _specialtiesService.GetById(model.Id);
            if (existing == null)
                return NotFound();

            if (imageFile != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(existing.ImagePath))
                    {
                        _imageManager.DeleteImage(existing.ImagePath);
                    }

                    model.ImagePath = _imageManager.SaveImage(imageFile, "specialties");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    return View(model);
                }
            }
            else
            {
                existing.ImagePath = existing.ImagePath;
            }
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.ImagePath = model.ImagePath;
            _specialtiesService.Update(existing);
            _notyf.Success("Specialty updated successfully!");

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var specialty = _specialtiesService.GetById(id);
            if (specialty == null)
                return Json(new { success = false, message = "Not found" });

            var hasDoctors = _doctorService.GetAll().Any(d => d.SpecialtyID == id);
            if (hasDoctors)
                return Json(new { success = false, message = "Cannot delete. Doctors assigned to this specialty." });

            if (!string.IsNullOrEmpty(specialty.ImagePath))
            {
                _imageManager.DeleteImage(specialty.ImagePath);
            }

            _specialtiesService.Delete(id);
            return Json(new { success = true });
        }

    }
}