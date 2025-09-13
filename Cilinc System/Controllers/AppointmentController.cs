using AspNetCoreHero.ToastNotification.Abstractions;
using Cilinc_System.Models;
using Cilinc_System.Models.Enums;
using Cilinc_System.Services.IServices;
using ClinicApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cilinc_System.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly ISpecialtiesService _specialtiesService;
        private readonly INotyfService _notyf;

        public AppointmentController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            ISpecialtiesService specialtiesService,
            INotyfService notyf)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _specialtiesService = specialtiesService;
            _notyf = notyf;
        }

        public IActionResult Index(
            string patientName = "",
            int? doctorId = null,
            AppointmentStatus? status = null,
            DateTime? date = null,
            int page = 1,
            int pageSize = 10)
        {
            var appointments = _appointmentService.GetAppointments();

            // Filter
            if (!string.IsNullOrEmpty(patientName))
                appointments = appointments.Where(a => a.Patient!.Name!.Contains(patientName, StringComparison.OrdinalIgnoreCase));

            if (doctorId.HasValue)
                appointments = appointments.Where(a => a.Doctor!.DoctorID == doctorId.Value);

            if (status.HasValue)
                appointments = appointments.Where(a => a.Status == status.Value);

            if (date.HasValue)
                appointments = appointments.Where(a => a.Date.Date == date.Value.Date);

            // Pagination
            var totalItems = appointments.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedAppointments = appointments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PatientFilter = patientName;
            ViewBag.DoctorFilter = doctorId;
            ViewBag.StatusFilter = status;
            ViewBag.DateFilter = date?.ToString("yyyy-MM-dd");

            var doctors = _doctorService.GetAll();
            ViewBag.Doctors = doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorID.ToString(),
                Text = d.Name,
                Selected = d.DoctorID.ToString() == doctorId.ToString()
            }).ToList();

            return View(pagedAppointments);
        }

        public IActionResult Create(int? doctorId)
        {
            ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name");

            if (doctorId.HasValue)
            {
                var doctor = _doctorService.GetById(doctorId.Value);
                if (doctor != null)
                {
                    ViewBag.SelectedDoctorId = doctor.DoctorID;
                    ViewBag.SelectedSpecialtyId = doctor.SpecialtyID;
                }
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment appointment, Patient patient)
        {
            if (appointment.Date.Date < DateTime.Today)
            {
                ModelState.AddModelError("appointment.Date", "You cannot book an appointment in the past.");
                ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", appointment.Doctor?.SpecialtyID);
                return View(appointment);
            }

            var validSlots = _appointmentService.GetFreeSlots(appointment.DoctorID, appointment.Date.Date);
            if (!validSlots.Contains(appointment.StartTime))
            {
                ModelState.AddModelError("appointment.StartTime", "Invalid appointment time. Please select a valid slot.");
                ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", appointment.Doctor?.SpecialtyID);
                return View(appointment);
            }

            if (ModelState.IsValid)
            {
                _appointmentService.CreateAppointmentWithPatient(appointment, patient);
                _notyf.Success("Appointment booked successfully!");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Specialties = new SelectList(_specialtiesService.GetAll(), "Id", "Name", appointment.Doctor?.SpecialtyID);
            return View(appointment);
        }


        // AJAX: Get doctors by specialty
        public IActionResult GetDoctorsBySpecialty(int specialtyId)
        {
            var doctors = _doctorService
                .GetAll()
                .Where(d => d.SpecialtyID == specialtyId)
                .Select(d => new { d.DoctorID, d.Name })
                .ToList();

            return Json(doctors);
        }



        public IActionResult GetFreeSlots(int doctorId, DateTime date)
        {
            if (date.Date < DateTime.Today)
            {
                return Json(new { success = false, message = "Booking in a past date is not allowed." });
            }

            var slots = _appointmentService.GetFreeSlots(doctorId, date);

            if (!slots.Any())
            {
                return Json(new { success = false, message = "Not available today." });
            }

            return Json(new { success = true, slots = slots.Select(s => s.ToString(@"hh\:mm")).ToList() });
        }

        [HttpGet]
        public IActionResult GetDoctorAppointments([FromQuery] List<int> doctorIds)
        {
            var appointmentsQuery = _appointmentService.GetAppointments().AsQueryable();

            if (doctorIds != null && doctorIds.Count > 0)
            {
                appointmentsQuery = appointmentsQuery.Where(a => doctorIds.Contains(a.DoctorID));
            }

            var appointments = appointmentsQuery
                .Select(a => new
                {
                    id = a.AppointmentID,
                    title = a.Patient!.Name + " ( DR. " + a.Doctor!.Name + " )",
                    start = a.Date.Add(a.StartTime),
                    end = a.Date.Add(a.EndTime),
                    status = a.Status.ToString()
                })
                .ToList();

            return Json(appointments);
        }


        // GET: Appointment/Details/5
        public IActionResult Details(int id)
        {
            var appointment = _appointmentService.GetAppointments()
                .FirstOrDefault(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, AppointmentStatus status)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return Json(new { success = false, message = "Appointment not found." });

            appointment.Status = status;
            _appointmentService.Update(appointment);

            return Json(new { success = true, message = "Status updated successfully." });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return Json(new { success = false, message = "Appointment not found." });

            _appointmentService.Delete(id);
            return Json(new { success = true, message = "Appointment deleted successfully." });
        }

        // GET: Appointment/Edit/5
        public IActionResult Edit(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
                return NotFound();

            ViewBag.Specialties = _specialtiesService.GetAll()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name,
                        Selected = appointment.Doctor != null && s.Id == appointment.Doctor.SpecialtyID
                    }).ToList();
            // List of doctors
            var doctors = _doctorService.GetAll();
            ViewBag.Doctors = doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorID.ToString(),
                Text = d.Name,
                Selected = d.DoctorID == appointment.DoctorID
            }).ToList();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Appointment appointment)
        {
            // Validate Specialty & Doctor
            if (appointment.DoctorID == 0)
            {
                ModelState.AddModelError("DoctorID", "Please select a doctor.");
            }

            if (appointment.Doctor?.SpecialtyID == 0)
            {
                ModelState.AddModelError("SpecialtyID", "Please select a specialty.");
            }
            if (ModelState.IsValid)
            {

                var existing = _appointmentService.GetById(appointment.AppointmentID);
                if (existing == null) return NotFound();

                if (existing.DoctorID != appointment.DoctorID || existing.Date.Date != appointment.Date.Date || existing.StartTime != appointment.StartTime)
                {
                    var validSlots = _appointmentService.GetFreeSlots(appointment.DoctorID, appointment.Date.Date);
                    if (!validSlots.Contains(appointment.StartTime))
                    {
                        ModelState.AddModelError("StartTime", "Invalid appointment time. Please select a valid slot.");
                        ViewBag.Specialties = _specialtiesService.GetAll()
                            .Select(s => new SelectListItem
                            {
                                Value = s.Id.ToString(),
                                Text = s.Name,
                                Selected = appointment.Doctor != null && s.Id == appointment.Doctor.SpecialtyID
                            }).ToList();

                        ViewBag.Doctors = _doctorService.GetAll()
                            .Select(d => new SelectListItem
                            {
                                Value = d.DoctorID.ToString(),
                                Text = d.Name,
                                Selected = appointment.DoctorID == d.DoctorID
                            }).ToList();

                        return View(appointment);
                    }
                }

                existing.Patient.Name = appointment.Patient.Name;
                existing.Date = appointment.Date;
                existing.StartTime = appointment.StartTime;
                existing.Status = appointment.Status;
                existing.DoctorID = appointment.DoctorID;

                _appointmentService.Update(existing);
                _notyf.Success("Appointment updated successfully!");

                return RedirectToAction(nameof(Index));
            }

            return View(appointment);
        }

    }
}
