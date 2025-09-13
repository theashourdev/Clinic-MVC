using AspNetCoreHero.ToastNotification.Abstractions;
using Cilinc_System.Models;
using Cilinc_System.Models.ViewModels;
using Cilinc_System.Services.IServices;
using ClinicApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cilinc_System.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISpecialtiesService _specialtiesService;
        private readonly IImageManager _imageManager;
        private readonly INotyfService _notyf;

        public DoctorController(IDoctorService doctorService, ISpecialtiesService specialtiesService, IImageManager imageManager, IAppointmentService appointmentService, INotyfService notyf)
        {
            _doctorService = doctorService;
            _specialtiesService = specialtiesService;
            _imageManager = imageManager;
            _appointmentService = appointmentService;
            _notyf = notyf;
        }

        // GET: Doctor
        public IActionResult Index()
        {
            var doctors = _doctorService.GetAll();
            return View(doctors);
        }

        // GET: Doctor/Details/5
        public IActionResult Details(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return PartialView("_DetailsPartial", doctor);
        }

        // GET: Doctor/Create
        public IActionResult Create()
        {
            ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name");

            var days = Enum.GetValues(typeof(DayOfWeek))
                           .Cast<DayOfWeek>()
                           .ToList();

            var model = new CreateDoctorViewModel
            {
                Schedules = days.Select(d => new DoctorScheduleViewModel
                {
                    DayOfWeek = d,
                    IsWorking = false
                }).ToList()
            };


            return View(model);
        }


        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Schedules == null || !model.Schedules.Any(s => s.IsWorking))
                {
                    ModelState.AddModelError("Schedules", "Please select at least one working day and its time.");
                    ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
                    return View(model);
                }

                var specialty = _specialtiesService.GetById(model.SpecialtyID);
                if (specialty == null)
                {
                    ModelState.AddModelError("SpecialtyID", "Invalid Specialty selected.");
                    ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
                    return View(model);
                }

                if (model.ImageFile != null)
                {
                    try
                    {
                        model.ImagePath = _imageManager.SaveImage(model.ImageFile, "doctors");
                    }
                    catch (ArgumentException ex)
                    {
                        ModelState.AddModelError("ImageFile", ex.Message);
                        ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
                        return View(model);
                    }
                }

                var doctor = new Doctor
                {
                    Name = model.Name,
                    SpecialtyID = model.SpecialtyID,
                    Phone = model.Phone,
                    Specialty = specialty,
                    ImagePath = model.ImagePath,
                    Schedules = model.Schedules
                        .Where(s => s.IsWorking)
                        .Select(s => new DoctorSchedule
                        {
                            DayOfWeek = s.DayOfWeek,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            IsWorking = s.IsWorking
                        }).ToList()
                };

                _doctorService.Create(doctor);
                _notyf.Success("Doctor created successfully!");

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
            return View(model);
        }


        public IActionResult Edit(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null)
                return NotFound();

            var days = Enum.GetValues(typeof(DayOfWeek))
                           .Cast<DayOfWeek>()
                           .ToList();

            var model = new EditDoctorViewModel
            {
                DoctorID = doctor.DoctorID,
                Name = doctor.Name,
                SpecialtyID = doctor.SpecialtyID,
                Phone = doctor.Phone,
                ImagePath = doctor.ImagePath,

                Schedules = days.Select(d =>
                {
                    var schedule = doctor.Schedules!.FirstOrDefault(s => s.DayOfWeek == d);
                    return schedule != null
                        ? new DoctorScheduleViewModel
                        {
                            DayOfWeek = schedule.DayOfWeek,
                            IsWorking = schedule.IsWorking,
                            StartTime = schedule.StartTime,
                            EndTime = schedule.EndTime
                        }
                        : new DoctorScheduleViewModel
                        {
                            DayOfWeek = d,
                            IsWorking = false
                        };
                }).ToList()
            };

            ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", doctor.SpecialtyID);

            return View(model);
        }


        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditDoctorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
                return View(model);
            }

            if (model.Schedules == null || !model.Schedules.Any(s => s.IsWorking))
            {
                ModelState.AddModelError("Schedules", "Please select at least one working day and its time.");
                ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
                return View(model);
            }

            var doctor = _doctorService.GetById(model.DoctorID);
            if (doctor == null)
                return NotFound();

            if (model.ImageFile != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(doctor.ImagePath))
                    {
                        _imageManager.DeleteImage(doctor.ImagePath);
                    }

                    model.ImagePath = _imageManager.SaveImage(model.ImageFile, "doctors");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("ImageFile", ex.Message);
                    ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", model.SpecialtyID);
                    return View(model);
                }
            }

            if (string.IsNullOrEmpty(model.ImagePath))
                model.ImagePath = doctor.ImagePath;

            doctor.Name = model.Name;
            doctor.Phone = model.Phone;
            doctor.SpecialtyID = model.SpecialtyID;
            doctor.ImagePath = model.ImagePath;

            _doctorService.UpdateWithSchedules(doctor);
            _notyf.Success("Doctor updated successfully!");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null)
                return Json(new { success = false, message = "Doctor not found." });

            var hasAppointments = _appointmentService.HasAppointmentsForDoctor(id);
            if (hasAppointments)
            {
                return Json(new { success = false, message = "Cannot delete doctor with existing appointments." });
            }

            if (!string.IsNullOrEmpty(doctor.ImagePath))
            {
                _imageManager.DeleteImage(doctor.ImagePath);
            }

            _doctorService.Delete(id);
            return Json(new { success = true });
        }



        // GET: Appointment/Calendar
        public IActionResult Calendar()
        {
            ViewBag.Doctors = new SelectList(_doctorService.GetAll(), "DoctorID", "Name");
            return View();
        }

    }
}
