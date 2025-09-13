using Cilinc_System.Models;

namespace ClinicApp.Services
{
    public interface IAppointmentService
    {
        // Get all appointments with patients & doctors
        IEnumerable<Appointment> GetAppointments();

        // Create new patient + appointment
        Appointment CreateAppointmentWithPatient(Appointment appointment, Patient patient);

        // Get free slots for a doctor in a specific date
        //IEnumerable<TimeSpan> GetFreeSlots(int doctorId, DateTime date);

        List<TimeSpan> GetFreeSlots(int doctorId, DateTime date);
        bool HasAppointmentsForDoctor(int doctorId);
        Appointment GetById(int id);
        void Update(Appointment appointment);
        void Delete(int id);
    }
}
