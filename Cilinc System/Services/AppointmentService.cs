using Cilinc_System.Models;
using Cilinc_System.Repositories.IRepositories;
using ClinicApp.IRepositories.IRepositories;

namespace ClinicApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IRepository<Patient> _patientRepo;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(IRepository<Patient> patientRepo, IAppointmentRepository appointmentRepo, IDoctorRepository doctorRepository)
        {
            _patientRepo = patientRepo;
            _appointmentRepo = appointmentRepo;
            _doctorRepository = doctorRepository;
        }

        // Existing
        public IEnumerable<Appointment> GetAppointments()
        {

            return _appointmentRepo.Get(null, includes: [i => i.Doctor!, i => i.Patient!, i => i.Doctor!.Specialty]);
        }

        // Create new patient + appointment
        public Appointment CreateAppointmentWithPatient(Appointment appointment, Patient patient)
        {
            var existingPatient = _patientRepo
                .Get(p => p.Phone == patient.Phone)
                .FirstOrDefault();

            if (existingPatient != null)
            {
                appointment.PatientID = existingPatient.PatientID;
            }
            else
            {
                _patientRepo.Create(patient);
                _patientRepo.Commit();
                appointment.PatientID = patient.PatientID;
            }

            appointment.EndTime = appointment.StartTime.Add(TimeSpan.FromMinutes(30));

            _appointmentRepo.Create(appointment);
            _appointmentRepo.Commit();

            return appointment;
        }


        public List<TimeSpan> GetFreeSlots(int doctorId, DateTime date)
        {
            // هات جدول الدكتور لليوم
            var doctor = _doctorRepository.GetOne(d => d.DoctorID == doctorId, includes: [d => d.Schedules!]);
            if (doctor == null) return new List<TimeSpan>();

            var daySchedule = doctor.Schedules!.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
            if (daySchedule == null || !daySchedule.IsWorking)
            {
                return new List<TimeSpan>();
            }

            var slotLength = TimeSpan.FromMinutes(30);
            var slots = new List<TimeSpan>();

            var bookedAppointments = _appointmentRepo.Get(
                a => a.DoctorID == doctorId && a.Date.Date == date.Date
            ).ToList();

            for (var time = daySchedule.StartTime; time < daySchedule.EndTime; time += slotLength)
            {
                var end = time + slotLength;

                bool isBooked = bookedAppointments.Any(a =>
                    (time >= a.StartTime && time < a.EndTime) ||   // overlap
                    (end > a.StartTime && end <= a.EndTime));

                if (!isBooked)
                {
                    slots.Add(time);
                }
            }

            return slots;
        }


        public bool HasAppointmentsForDoctor(int doctorId)
        {
            return _appointmentRepo.HasAppointmentsForDoctor(doctorId);
        }

        public Appointment GetById(int id)
        {
            return _appointmentRepo.GetOne(a => a.AppointmentID == id, includes: [a => a.Doctor!, a => a.Patient!])!;
        }


        public void Update(Appointment appointment)
        {
            _appointmentRepo.Edit(appointment);
            _appointmentRepo.Commit();
        }

        public void Delete(int id)
        {
            var appointment = GetById(id);
            if (appointment == null)
                throw new ArgumentException("Appointment not found.");
            _appointmentRepo.Delete(appointment);
            _appointmentRepo.Commit();
        }
    }

}
