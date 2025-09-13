using Cilinc_System.Models;

namespace ClinicApp.Services
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAppointments();
        Appointment CreateAppointmentWithPatient(Appointment appointment, Patient patient);
        List<TimeSpan> GetFreeSlots(int doctorId, DateTime date);
        bool HasAppointmentsForDoctor(int doctorId);
        Appointment GetById(int id);
        void Update(Appointment appointment);
        void Delete(int id);
    }
}
